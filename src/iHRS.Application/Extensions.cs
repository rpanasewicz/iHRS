using AutoMapper;
using iHRS.Application.Common;
using iHRS.Application.Queries;
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

            services.AddScoped<EmployeeQueries>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
