using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Application.DomainEvents;
using iHRS.Application.Queries;
using iHRS.Application.Services;
using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Infrastructure.Auth;
using iHRS.Infrastructure.Decorators;
using iHRS.Infrastructure.Dispatchers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Scrutor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace iHRS.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(typeof(ReservationCreatedDomainEventHandler).Assembly)
                        .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(typeof(ICommandHandler<,>).Assembly)
                        .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(IQuery)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(IService)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());


            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IDomainEventPublisher, DomainEventDispatcher>();

            services.AddJwt();
            services.AddScoped<IAuthProvider, AuthProvider>();

            services.AddDbContext<HRSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Extensions).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            services.AddCommandDecorator(typeof(ICommandHandler<,>), typeof(CommandHandlerDomainEventDispatcherDecorator<,>));
            services.AddCommandDecorator(typeof(ICommandHandler<,>), typeof(CommandHandlerTransactionDecorator<,>));
            services.AddCommandDecorator(typeof(ICommandHandler<,>), typeof(CommandHandlerLoggingDecorator<,>));
            services.AddDomainEventHandlerDecorator(typeof(IDomainEventHandler<>), typeof(DomainEventLoggingDecorator<>));

            return services;
        }

        public static IWebHost MigrateDatabases(this IWebHost webHost)
        {
            return MigrateDbContext<HRSContext>(webHost, (context, provider) =>
            {
                context.SeedEnumerations();
                context.SaveChanges();
            });
        }

        private static void SeedEnumerations(this HRSContext context, Assembly assembly = null)
        {
            assembly ??= typeof(Enumeration).Assembly;

            assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Enumeration)) && t.IsClass && !t.IsAbstract)
                .ToList()
                .ForEach(t =>
                {
                    var enumValues = typeof(Enumeration)
                        .GetMethod("GetAll")?
                        .MakeGenericMethod(t)
                        .Invoke(null, null);

                    var set = context
                        .GetType()
                        .GetMethods()
                        .FirstOrDefault(m => m.Name == "Set" && m.GetParameters().Length == 0)?
                        .MakeGenericMethod(t)
                        .Invoke(context, null) as IListSource;

                    var findResult = set?
                        .GetType()?
                        .GetMethod("Find")?
                        .Invoke(set, new object[] { new object[] { 1 } });

                    if (findResult is null)
                        set?
                        .GetType()
                        .GetMethods()
                        .FirstOrDefault(m => m.Name == "AddRange" && m.GetParameters().First().ParameterType.Name.StartsWith("IEnumerable"))?
                        .Invoke(set, new[] { enumValues });
                });
        }

        internal static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retries = 10;
                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                        retryCount: retries,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, timeSpan, retry, ctx) =>
                        {
                            logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", nameof(TContext), exception.GetType().Name, exception.Message, retry, retries);
                        });

                retry.Execute(() =>
                {
                    context?.Database.Migrate();
                    seeder?.Invoke(context, services);
                });


                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }

            return webHost;
        }

        private static IServiceCollection AddCommandDecorator(this IServiceCollection services, Type handlerType, Type decoratorType)
        {
            var handlers = handlerType.Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                .ToList();

            handlers.ForEach(ch =>
            {
                GetExtensionMethods()
                    .FirstOrDefault(mi => !mi.IsGenericMethod && mi.Name == "TryDecorate")?
                    .Invoke(services, new object[]
                    {
                            services,
                            ch.GetInterfaces().FirstOrDefault(il => il.GenericTypeArguments.Length == 2),
                            decoratorType.MakeGenericType(ch.GetInterfaces().FirstOrDefault(il => il.GenericTypeArguments.Length == 2)?.GenericTypeArguments ?? new Type[] { })
                    });
            });

            return services;
        }

        private static IServiceCollection AddDomainEventHandlerDecorator(this IServiceCollection services, Type handlerType, Type decoratorType)
        {
            var handlers = typeof(ReservationCreatedDomainEventHandler).Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                .ToList();

            handlers.ForEach(ch =>
            {
                GetExtensionMethods()
                    .FirstOrDefault(mi => !mi.IsGenericMethod && mi.Name == "TryDecorate")?
                    .Invoke(services, new object[]
                    {
                        services,
                        ch.GetInterfaces().FirstOrDefault(il => il.GenericTypeArguments.Length == 1),
                        decoratorType.MakeGenericType(ch.GetInterfaces().FirstOrDefault(il => il.GenericTypeArguments.Length == 1)?.GenericTypeArguments ?? new Type[] { })
                    });
            });

            return services;
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods()
        {
            var types = typeof(ReplacementBehavior).Assembly.GetTypes();

            var query = from type in types
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                        select method;
            return query;
        }
    }

}
