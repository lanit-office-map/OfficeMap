using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
    public interface IDelete<in TEntity>
    {
        Task DeleteAsync(TEntity entity);
    }
}

