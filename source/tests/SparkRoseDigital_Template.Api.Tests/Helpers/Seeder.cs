using System.Collections.Generic;
using SparkRoseDigital_Template.Core.Entities;
using SparkRoseDigital_Template.Data;

namespace SparkRoseDigital_Template.Api.Tests.Helpers
{
    public static class Seeder
    {
        public static void Seed(this SparkRoseDigital_TemplateDbContext ctx)
        {
            ctx.Foos.AddRange(
                new List<Foo>
                {
                    new ("Text 1"),
                    new ("Text 2"),
                    new ("Text 3")
                });
            ctx.SaveChanges();
        }
    }
}
