using System.Threading.Tasks;
using SparkRoseDigital_Template.Common.Interfaces;

namespace SparkRoseDigital_Template.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SparkRoseDigital_TemplateDbContext _dbContext;

        public UnitOfWork(SparkRoseDigital_TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveAsync()
        {
            if (_dbContext.ChangeTracker.HasChanges())
            {
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}