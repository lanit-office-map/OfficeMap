using System.Threading.Tasks;

namespace MapService.Interfaces
{
    public interface ICreate<TEntity>
       where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
    }
}
