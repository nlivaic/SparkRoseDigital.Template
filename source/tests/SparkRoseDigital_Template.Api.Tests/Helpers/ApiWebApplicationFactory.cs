using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SparkRoseDigital_Template.Data;
using Xunit;

namespace SparkRoseDigital_Template.Api.Tests.Helpers
{
    public class ApiWebApplicationFactory :
        WebApplicationFactory<Startup>,
        IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;

        public string ConnectionString => _msSqlContainer.ConnectionString;

        public ApiWebApplicationFactory()
        {
            _msSqlContainer = new MsSqlContainer();
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
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SparkRoseDigital_TemplateDbContext>)));
                services.Remove(services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection)));
                services.AddDbContext<SparkRoseDigital_TemplateDbContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.ConnectionString);
                });
                services.AddMassTransitTestHarness();
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var ctx = scopedServices.GetRequiredService<SparkRoseDigital_TemplateDbContext>();
                ctx.Database.EnsureCreated();
                ctx.Seed();
                ctx.SaveChanges();
            });
        }

        public async Task InitializeAsync() =>
            await _msSqlContainer.InitializeAsync();

        public async Task DisposeAsync() =>
            await _msSqlContainer.DisposeAsync();

    }
}
