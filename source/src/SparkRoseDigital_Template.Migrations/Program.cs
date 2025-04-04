﻿using System;
using System.IO;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SparkRoseDigital_Template.Migrations;

public class Program
{
    public static int Main(string[] args)
    {
        var connectionString = string.Empty;
        var dbUser = string.Empty;
        var dbPassword = string.Empty;
        var scriptsPath = string.Empty;
        var sqlUsersGroupName = string.Empty;

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";
        Console.WriteLine($"Environment: {env}.");
        var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env}.json", true, true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        InitializeParameters();
        var connectionStringBuilderSparkRoseDigital_Template = new SqlConnectionStringBuilder(connectionString);
        if (env.Equals("Development"))
        {
            connectionStringBuilderSparkRoseDigital_Template.UserID = dbUser;
            connectionStringBuilderSparkRoseDigital_Template.Password = dbPassword;
        }
        else
        {
            connectionStringBuilderSparkRoseDigital_Template.UserID = dbUser;
            connectionStringBuilderSparkRoseDigital_Template.Password = dbPassword;
            connectionStringBuilderSparkRoseDigital_Template.Authentication = SqlAuthenticationMethod.ActiveDirectoryPassword;
        }
        var upgraderSparkRoseDigital_Template =
            DeployChanges.To
                .SqlDatabase(connectionStringBuilderSparkRoseDigital_Template.ConnectionString)
                .WithVariable("SqlUsersGroupNameVariable", sqlUsersGroupName)    // This is necessary to perform template variable replacement in the scripts.
                .WithScriptsFromFileSystem(
                    !string.IsNullOrWhiteSpace(scriptsPath)
                            ? Path.Combine(scriptsPath, "SparkRoseDigital_TemplateScripts")
                        : Path.Combine(Environment.CurrentDirectory, "SparkRoseDigital_TemplateScripts"))
                .LogToConsole()
                .Build();
        Console.WriteLine($"Now upgrading SparkRoseDigital_Template.");
        if (env == "Development")
        {
            upgraderSparkRoseDigital_Template.MarkAsExecuted("0000_AzureSqlContainedUser.sql");
        }
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
            // Local database, populated from .env file.
            if (args.Length == 0)
            {
                connectionString = config["SparkRoseDigital_TemplateDb_Migrations_Connection"];
                dbUser = config["DbUser"];
                dbPassword = config["DbPassword"];
            }

            // Deployed database
            else if (args.Length == 5)
            {
                connectionString = args[0];
                dbUser = args[1];
                dbPassword = args[2];
                scriptsPath = args[3];
                sqlUsersGroupName = args[4];
            }
        }
    }
}
