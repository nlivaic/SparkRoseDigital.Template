using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using SparkRoseDigital.Infrastructure.Logging;

namespace SparkRoseDigital_Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SparkRoseDigital.Infrastructure.Logging.LoggerExtensions.ConfigureSerilogLogger("ASPNETCORE_ENVIRONMENT");

            try
            {
                Log.Information("Starting up SparkRoseDigital_Template.");
                CreateHostBuilder(args)
                    .Build()
                    .AddW3CTraceContextActivityLogging()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "SparkRoseDigital_Template failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
