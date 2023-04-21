using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using SparkRoseDigital_Template.Api.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SparkRoseDigital_Template.Api.Tests
{
    public class FooTest : IClassFixture<MsSqlTests>
    {
        private readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;
        private readonly ITestOutputHelper _testOutput;

        public FooTest(
            ITestOutputHelper testOutput,
            MsSqlTests fixture)
        {
            _webApplicationFactory = new CustomWebApplicationFactory<Startup>(fixture);
            _webApplicationFactory.ClientOptions.BaseAddress = new Uri("http://localhost/api/");
            _testOutput = testOutput;
        }

        [Fact]
        public async Task FooTest1()
        {
            // Arrange
            var client = _webApplicationFactory.CreateClient();
            using var ctx = new MsSqlDbBuilder(_testOutput, _webApplicationFactory.ConnectionString)
                .BuildContext();
            var initialCount = ctx.Foos.Count();

            // Act
            var response = await client.PostAsJsonAsync("foos", new
            {
                Text = "My_Test_Title"
            });
            using var ctx1 = new MsSqlDbBuilder(_testOutput, _webApplicationFactory.ConnectionString)
                .BuildContext();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(initialCount + 1, ctx1.Foos.Count());
        }
    }
}
