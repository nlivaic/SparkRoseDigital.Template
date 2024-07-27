using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using SparkRoseDigital.Infrastructure.Logging;
using LoggerExtensions = SparkRoseDigital.Infrastructure.Logging.LoggerExtensions;

namespace SparkRoseDigital_Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerExtensions.ConfigureSerilogLogger("ASPNETCORE_ENVIRONMENT");

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
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();
                    if (!string.IsNullOrEmpty(builtConfig["KeyVault:Uri"]))
                    {
                        config.AddAzureKeyVault(
                            new Uri(builtConfig["KeyVault:Uri"]),
                            new DefaultAzureCredential());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
