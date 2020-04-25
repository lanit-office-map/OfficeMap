using System.Threading.Tasks;

namespace MapService.Interfaces
{
    public interface IUpdate<TEntity>
        where TEntity : class
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}

