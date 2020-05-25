using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUpdate<TEntity>
        where TEntity : class
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
