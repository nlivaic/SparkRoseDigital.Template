using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SparkRoseDigital_Template.Data;
using Xunit.Abstractions;

namespace SparkRoseDigital_Template.Api.Tests.Helpers
{
    public class MsSqlDbBuilder
    {
        private readonly DbContextOptions<SparkRoseDigital_TemplateDbContext> _options;

        /// <summary>
        /// Creates a new DbContext with an open database connection already set up.
        /// Make sure not to call `context.Database.OpenConnection()` from your code.
        /// </summary>
        public MsSqlDbBuilder(
            ITestOutputHelper testOutput,
            string connection,
            List<string> logs = null)   // This parameter is just for demo purposes, to show you can output logs.
        {
            _options = new DbContextOptionsBuilder<SparkRoseDigital_TemplateDbContext>()
                .UseLoggerFactory(new LoggerFactory(
                    new[] {
                        new TestLoggerProvider(
                            message => testOutput.WriteLine(message),
                            // message => logs?.Add(message),
                            LogLevel.Information
                        )
                    }
                ))
                .UseSqlServer(connection)
                .Options;
        }

        internal SparkRoseDigital_TemplateDbContext BuildContext() =>
            new (_options);
    }
}
