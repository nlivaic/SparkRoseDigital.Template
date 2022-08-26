using MassTransit;
using Microsoft.EntityFrameworkCore;
using SparkRoseDigital_Template.Core.Entities;

namespace SparkRoseDigital_Template.Data
{
    public class SparkRoseDigital_TemplateDbContext : DbContext
    {
        public SparkRoseDigital_TemplateDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Foo> Foos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
