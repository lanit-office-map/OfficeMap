using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<in TEntity>
    {
        Task DeleteAsync(TEntity entity);
    }
}
