using System;
using System.Reflection;
using Azure.Monitor.OpenTelemetry.Exporter;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SparkRoseDigital.Infrastructure.Caching;
using SparkRoseDigital.Infrastructure.Logging;
using SparkRoseDigital_Template.Common.MessageBroker;
using SparkRoseDigital_Template.Common.MessageBroker.Middlewares.ErrorLogging;
using SparkRoseDigital_Template.Common.MessageBroker.Middlewares.Tracing;
using SparkRoseDigital_Template.Core;
using SparkRoseDigital_Template.Core.Events;
using SparkRoseDigital_Template.Data;
using SparkRoseDigital_Template.WorkerServices.FaultService;
using SparkRoseDigital_Template.WorkerServices.FooService;
using LoggerExtensions = SparkRoseDigital.Infrastructure.Logging.LoggerExtensions;

namespace SparkRoseDigital_Template.WorkerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerExtensions.ConfigureSerilogLogger("DOTNET_ENVIRONMENT");

            try
            {
                Log.Information("Starting up SparkRoseDigital_Template Worker Services.");
                CreateHostBuilder(args)
                    .Build()
                    .AddW3CTraceContextActivityLogging()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "SparkRoseDigital_Template Worker Services failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var hostEnvironment = hostContext.HostingEnvironment;
                    services.AddDbContext<SparkRoseDigital_TemplateDbContext>(options =>
                    {
                        var connString = new SqlConnectionStringBuilder(configuration.GetConnectionString("SparkRoseDigital_TemplateDbConnection"))
                        {
                            UserID = configuration["DB_USER"],
                            Password = configuration["DB_PASSWORD"]
                        };
                        options.UseSqlServer(connString.ConnectionString);
                        if (hostEnvironment.IsDevelopment())
                        {
                            options.EnableSensitiveDataLogging(true);
                        }
                    });
                    services.AddGenericRepository();
                    services.AddSpecificRepositories();
                    services.AddCoreServices();
                    services.AddSingleton<ICache, Cache>();
                    services.AddMemoryCache();
                    services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(Program).Assembly);

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FooConsumer>();
                        x.AddConsumer<FooFaultConsumer>();
                        x.AddConsumer<FaultConsumer>();
                        x.AddConsumer<FooCommandConsumer>(typeof(FooCommandConsumer.FooCommandConsumerDefinition));
                        x.UsingAzureServiceBus((ctx, cfg) =>
                        {
                            cfg.Host(
                                new MessageBrokerConnectionStringBuilder(
                                    configuration.GetConnectionString("MessageBroker"),
                                    configuration["MessageBroker:Reader:SharedAccessKeyName"],
                                    configuration["MessageBroker:Reader:SharedAccessKey"]).ConnectionString);

                            // Use the below line if you are not going with
                            // SetKebabCaseEndpointNameFormatter() in the publishing project (see API project),
                            // but have rather given the topic a custom name.
                            // cfg.Message<VoteCast>(configTopology => configTopology.SetEntityName("foo-topic"));
                            cfg.SubscriptionEndpoint<IFooEvent>("foo-event-subscription-1", e =>
                            {
                                e.ConfigureConsumer<FooConsumer>(ctx);
                            });

                            // This is here only for show. I have not thought through a proper
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault<IFooEvent>>("foo-event-fault-consumer", e =>
                            {
                                e.ConfigureConsumer<FooFaultConsumer>(ctx);
                            });

                            // This is here only for show. I have not thought through a proper
                            // error handling strategy.
                            cfg.SubscriptionEndpoint<Fault>("fault-consumer", e =>
                            {
                                e.ConfigureConsumer<FaultConsumer>(ctx);
                            });
                            cfg.ConfigureEndpoints(ctx);

                            cfg.UseMessageBrokerTracing();
                            cfg.UseExceptionLogger(services);
                        });
                        x.AddEntityFrameworkOutbox<SparkRoseDigital_TemplateDbContext>(o =>
                        {
                            // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                            o.UseSqlServer();

                            // enable the bus outbox
                            o.UseBusOutbox();
                        });
                    });
                    if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
                    {
                        services
                            .AddOpenTelemetry()
                            .WithTracing(tracerProviderBuilder =>
                            {
                                tracerProviderBuilder
                                    .AddSource(WorkerAssemblyInfo.Value.GetName().Name)
                                    .SetResourceBuilder(
                                        ResourceBuilder
                                            .CreateDefault()
                                            .AddService(serviceName: WorkerAssemblyInfo.Value.GetName().Name))
                                    .AddEntityFrameworkCoreInstrumentation()
                                    .AddSqlClientInstrumentation()
                                    .AddSource("MassTransit")
                                    .AddAzureMonitorTraceExporter(o =>
                                    {
                                        o.ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
                                    });
                            })//.WithMetrics(meterProviderBuilder =>
                              //{
                              //    meterProviderBuilder
                              //        .SetResourceBuilder(
                              //            ResourceBuilder
                              //                .CreateDefault()
                              //                .AddService(serviceName: "TestTemplate2"))
                              //        .AddAspNetCoreInstrumentation()
                              //        .AddAzureMonitorMetricExporter(o =>
                              //        {
                              //            //o.ConnectionString = "InstrumentationKey=f051d7dd-dbaf-450a-a6f1-9f78bc0f8c91";
                              //            o.ConnectionString = "InstrumentationKey=f051d7dd-dbaf-450a-a6f1-9f78bc0f8c91;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";
                              //        })
                              //        .AddConsoleExporter();
                              //})
                            .StartWithHost();
                    }
                });
    }
}
