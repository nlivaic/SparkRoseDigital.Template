using SparkRoseDigital_Template.Core.Entities;
using SparkRoseDigital_Template.Core.Interfaces;

namespace SparkRoseDigital_Template.Data.Repositories;

public class FooRepository : Repository<Foo>, IFooRepository
{
    public FooRepository(SparkRoseDigital_TemplateDbContext context)
        : base(context)
    {
    }
}
