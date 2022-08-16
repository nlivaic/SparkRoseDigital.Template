using System;
using System.Linq;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SparkRoseDigital_Template.Data;

namespace SparkRoseDigital_Template.Api.Tests.Helpers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public SqliteConnection KeepAliveConnection { get; }

        public CustomWebApplicationFactory()
        {
            KeepAliveConnection = new SqliteConnection("DataSource=:memory:");
            KeepAliveConnection.Open();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            }
            Environment.SetEnvironmentVariable("MessageBroker__Writer__SharedAccessKeyName", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Writer__SharedAccessKey", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Reader__SharedAccessKeyName", "Test");
            Environment.SetEnvironmentVariable("MessageBroker__Reader__SharedAccessKey", "Test");
            return base.CreateHostBuilder()
                .ConfigureHostConfiguration(
                    config => config.AddEnvironmentVariables("ASPNETCORE"));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .AddAuthentication("Test")
                    .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test", null);
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<SparkRoseDigital_TemplateDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<SparkRoseDigital_TemplateDbContext>(options =>
                {
                    options.UseSqlite(KeepAliveConnection);
                });
                services.AddMassTransitTestHarness();
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                //var harness = scopedServices.GetRequiredService<ITestHarness>();
                //harness.Start().GetAwaiter().GetResult();
                var ctx = scopedServices.GetRequiredService<SparkRoseDigital_TemplateDbContext>();
                ctx.Database.EnsureCreated();
                ctx.Seed();
                ctx.SaveChanges();
            });
        }
    }
}
