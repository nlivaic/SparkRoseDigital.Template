using Microsoft.Extensions.DependencyInjection;
using SparkRoseDigital_Template.Application.Pipelines;

namespace SparkRoseDigital_Template.Application;

public static class ServiceCollectionExtensions
{
    public static void AddSparkRoseDigital_TemplateApplicationHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));
        services.AddPipelines();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
