﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace SparkRoseDigital_Template.Common.MessageBroker.Middlewares.ErrorLogging;

public static class ExampleMiddlewareConfiguratorExtensions
{
    public static void UseExceptionLogger<T>(this IPipeConfigurator<T> configurator, IServiceCollection serviceCollection)
        where T : class, PipeContext => configurator.AddPipeSpecification(new ExceptionLoggerSpecification<T>(serviceCollection));
}
