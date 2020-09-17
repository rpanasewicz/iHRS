using iHRS.Api.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
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

                })
                .ConfigureServices(services =>
                {

                })
                .Configure(app =>
                {
                    app.UseMiddleware<ErrorHandlerMiddleware>();
                    app.UseRouting();

                    app.UseCors("allow-all");
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                })
                .Build()
                .RunAsync();
        }
    }
}

