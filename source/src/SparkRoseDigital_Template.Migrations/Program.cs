using System;
using System.IO;
using System.Linq;
using DbUp;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SparkRoseDigital_Template.Migrations
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";
            Console.WriteLine($"Environment: {env}.");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connectionStringSparkRoseDigital_Template = new NpgsqlConnectionStringBuilder(
                string.IsNullOrWhiteSpace(args.FirstOrDefault())
                    ? config["ConnectionStrings:SparkRoseDigital_TemplateDbConnection"]
                    : args.FirstOrDefault())
            {
                Username = config["DB_USER"],
                Password = config["DB_PASSWORD"]
            }.ConnectionString;

            string scriptsPath = null;
            if (args.Length == 3)
            {
                scriptsPath = args[2];
            }

            var upgraderSparkRoseDigital_Template =
                DeployChanges.To
                    .PostgresqlDatabase(connectionStringSparkRoseDigital_Template)
                    .WithScriptsFromFileSystem(
                        !string.IsNullOrWhiteSpace(scriptsPath)
                                ? Path.Combine(scriptsPath, "SparkRoseDigital_TemplateScripts")
                            : Path.Combine(Environment.CurrentDirectory, "SparkRoseDigital_TemplateScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading SparkRoseDigital_Template.");
            if (env != "Development")
            {
                Console.WriteLine($"Skipping 0005_InitialData.sql since we are not in Development environment.");
                upgraderSparkRoseDigital_Template.MarkAsExecuted("0005_InitialData.sql");
            }
            var resultSparkRoseDigital_Template = upgraderSparkRoseDigital_Template.PerformUpgrade();

            if (!resultSparkRoseDigital_Template.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"SparkRoseDigital_Template upgrade error: {resultSparkRoseDigital_Template.Error}");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            // Uncomment the below sections if you also have an Identity Server project in the solution.
            /*
            var connectionStringSparkRoseDigital_TemplateIdentity = string.IsNullOrWhiteSpace(args.FirstOrDefault())
                ? config["ConnectionStrings:SparkRoseDigital_TemplateIdentityDb"]
                : args.FirstOrDefault();

            var upgraderSparkRoseDigital_TemplateIdentity =
                DeployChanges.To
                    .PostgresqlDatabase(connectionStringSparkRoseDigital_TemplateIdentity)
                    .WithScriptsFromFileSystem(
                        scriptsPath != null
                            ? Path.Combine(scriptsPath, "SparkRoseDigital_TemplateIdentityScripts")
                            : Path.Combine(Environment.CurrentDirectory, "SparkRoseDigital_TemplateIdentityScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading SparkRoseDigital_Template Identity.");
            if (env != "Development")
            {
                upgraderSparkRoseDigital_TemplateIdentity.MarkAsExecuted("0004_InitialData.sql");
                Console.WriteLine($"Skipping 0004_InitialData.sql since we are not in Development environment.");
                upgraderSparkRoseDigital_TemplateIdentity.MarkAsExecuted("0005_Initial_Configuration_Data.sql");
                Console.WriteLine($"Skipping 0005_Initial_Configuration_Data.sql since we are not in Development environment.");
            }
            var resultSparkRoseDigital_TemplateIdentity = upgraderSparkRoseDigital_TemplateIdentity.PerformUpgrade();

            if (!resultSparkRoseDigital_TemplateIdentity.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"SparkRoseDigital_Template Identity upgrade error: {resultSparkRoseDigital_TemplateIdentity.Error}");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }
            */

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
