using System.Threading.Tasks;

namespace MapService.Interfaces
{
    public interface IGet<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);
    }
}
