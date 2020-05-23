using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface IUpdate<TEntity>
        where TEntity : class
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}