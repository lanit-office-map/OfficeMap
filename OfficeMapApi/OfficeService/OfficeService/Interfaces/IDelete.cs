using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
    public interface IDelete<in TEntity>
        where TEntity : class
    {
        Task DeleteAsync(TEntity entity);
    }

}

