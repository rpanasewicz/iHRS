using iHRS.Application.Common;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
