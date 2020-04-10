using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
    public interface IDelete<in TEntity>
        where TEntity : class
    {
        Task DeleteAsync(TEntity entity);
    }
}

