using System.Threading.Tasks;

namespace SparkRoseDigital_Template.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}