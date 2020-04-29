using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICreate<TEntity>
       where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
    }
}
