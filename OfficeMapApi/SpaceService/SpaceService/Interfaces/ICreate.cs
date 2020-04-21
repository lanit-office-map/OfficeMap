using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface ICreate<TEntity>
       where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
    }
}