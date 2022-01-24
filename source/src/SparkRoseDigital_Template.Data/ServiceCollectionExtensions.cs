using Microsoft.Extensions.DependencyInjection;
using SparkRoseDigital_Template.Common.Interfaces;
using SparkRoseDigital_Template.Core.Interfaces;
using SparkRoseDigital_Template.Data.Repositories;

namespace SparkRoseDigital_Template.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSpecificRepositories(this IServiceCollection services) =>
            services.AddScoped<IFooRepository, FooRepository>();

        public static void AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
