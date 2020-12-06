using iHRS.Api.Binders;
using iHRS.Api.Exceptions;
using iHRS.Application;
using iHRS.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Threading.Tasks;

namespace iHRS.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost
                .CreateDefaultBuilder(args)
                .UseSerilog((context, loggerConfiguration) =>
                {      
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);

                    loggerConfiguration.Enrich.FromLogContext()
                        .Enrich.WithProperty("Environment", "Development")
                        .Enrich.WithProperty("Application", "iHRS")
                        .Enrich.WithProperty("Version", "0.0");
                })
                .ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                    services.AddApplication();
                    services.AddInfrastructure(configuration);

                    services.AddControllers(opts =>
                    {
                        opts.ModelBinderProviders.InsertBodyAndRouteBinding();
                    });

                    services.AddCors(options =>
                    {
                        options.AddPolicy(name: "allow-all",
                            builder =>
                            {
                                builder
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowAnyOrigin();
                            });
                    });

                    services.AddMvc();
                    services.AddTransient<ErrorHandlerMiddleware>();
                })
                .Configure(app =>
                {
                    app.UseHttpsRedirection();
                    app.UseMiddleware<ErrorHandlerMiddleware>();
                    app.UseRouting();
                    app.UseAuthorization();
                    app.UseCors("allow-all");
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                })
                .Build()
                .MigrateDatabases()
                .RunAsync();
        }
    }
}

