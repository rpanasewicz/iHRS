using System;
using iHRS.Api.Exceptions;
using iHRS.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.Threading.Tasks;
using iHRS.Application;
using iHRS.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

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
                    loggerConfiguration.WriteTo.Console();

                    loggerConfiguration.Enrich.FromLogContext()
                        .MinimumLevel.Is(LogEventLevel.Information)
                        .Enrich.WithProperty("Environment", "Development")
                        .Enrich.WithProperty("Application", "iHRS")
                        .Enrich.WithProperty("Version", "0.0");
                })
                .ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
                    services.AddApplication();
                    services.AddInfrastructure(configuration);

                    services.AddControllers();

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

                    services.AddTransient<ErrorHandlerMiddleware>();

                })
                .Configure(app =>
                {
                    app.UseMiddleware<ErrorHandlerMiddleware>();
                    app.UseRouting();

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

