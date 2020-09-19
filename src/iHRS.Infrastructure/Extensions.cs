using iHRS.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using iHRS.Application.Common;
using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Infrastructure.Dispatchers;
using Microsoft.Extensions.Configuration;

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
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());

            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());


            services
                .Scan(scan =>
                    scan
                        .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                        .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());


            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IDomainEventPublisher, DomainEventDispatcher>();

            services.AddDbContext<HRSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Extensions).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            return services;
        }

        public static IWebHost MigrateDatabases(this IWebHost webHost)
        {
            return MigrateDbContext<HRSContext>(webHost, (context, provider) =>
            {

                context.SaveChanges();
            });
        }

        private static void SeedEnumeration<T>(this HRSContext context) where T : Enumeration
        {
            context.SeedEnumeration<T, int>();
        }

        private static void SeedEnumeration<T, TKey>(this HRSContext context) where T : Enumeration<TKey> where TKey : IComparable, IEquatable<TKey>
        {
            if (context.Set<T>().Any()) return;
            context.Set<T>().AddRange(Enumeration<TKey>.GetAll<T>());
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

    }
}
