using System;
using System.IO;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SparkRoseDigital_Template.Migrations
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var connectionString = string.Empty;
            var dbUser = string.Empty;
            var dbPassword = string.Empty;
            var scriptsPath = string.Empty;

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";
            Console.WriteLine($"Environment: {env}.");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            InitializeParameters();
            var connectionStringSparkRoseDigital_Template = new SqlConnectionStringBuilder(connectionString)
            {
                UserID = dbUser,
                Password = dbPassword
            }.ConnectionString;

            var upgraderSparkRoseDigital_Template =
                DeployChanges.To
                    .SqlDatabase(connectionStringSparkRoseDigital_Template)
                    .WithScriptsFromFileSystem(
                        !string.IsNullOrWhiteSpace(scriptsPath)
                                ? Path.Combine(scriptsPath, "SparkRoseDigital_TemplateScripts")
                            : Path.Combine(Environment.CurrentDirectory, "SparkRoseDigital_TemplateScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading SparkRoseDigital_Template.");
            var resultSparkRoseDigital_Template = upgraderSparkRoseDigital_Template.PerformUpgrade();

            if (!resultSparkRoseDigital_Template.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"SparkRoseDigital_Template upgrade error: {resultSparkRoseDigital_Template.Error}");
                Console.ResetColor();
                return -1;
            }

            // Uncomment the below sections if you also have an Identity Server project in the solution.
            /*
            var connectionStringSparkRoseDigital_TemplateIdentity = string.IsNullOrWhiteSpace(args.FirstOrDefault())
                ? config["ConnectionStrings:SparkRoseDigital_TemplateIdentityDb"]
                : args.FirstOrDefault();

            var upgraderSparkRoseDigital_TemplateIdentity =
                DeployChanges.To
                    .SqlDatabase(connectionStringSparkRoseDigital_TemplateIdentity)
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
                return -1;
            }
            */

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;

            void InitializeParameters()
            {
                if (args.Length == 0)
                {
                    connectionString = config["ConnectionStrings:SparkRoseDigital_TemplateDb_Migrations_Connection"];
                    dbUser = config["DB_USER"];
                    dbPassword = config["DB_PASSWORD"];
                }
                else if (args.Length == 4)
                {
                    connectionString = args[0];
                    dbUser = args[1];
                    dbPassword = args[2];
                    scriptsPath = args[3];
                }
            }
        }
    }
}
