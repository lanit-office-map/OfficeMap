using System.Threading.Tasks;

namespace MapService.Interfaces
{
    public interface IDelete<in TEntity>
    {
        Task DeleteAsync(TEntity entity);
    }
}
