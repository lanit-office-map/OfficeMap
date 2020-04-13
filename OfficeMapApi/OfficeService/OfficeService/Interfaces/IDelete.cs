using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
    public interface IDelete<in TEntity>
    {
        Task DeleteAsync(TEntity entity);
    }

}

