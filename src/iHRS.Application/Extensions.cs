using AutoMapper;
using iHRS.Application.Common;
using iHRS.Application.Queries;
using iHRS.Application.Queries.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace iHRS.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.Scan(s =>
                s.FromAssemblies(typeof(ICommandHandler<>).Assembly)
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.Scan(s =>
                s.FromAssemblies(typeof(IQuery).Assembly)
                    .AddClasses(c => c.AssignableTo(typeof(IQuery)))
                    .AsSelf()
                    .WithScopedLifetime());

            services.AddScoped<EmployeeQueries>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
