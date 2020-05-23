using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface IGet<TEntity, TFilter, in TKey>
        where TEntity : class
        where TFilter : class
    {
        Task<TEntity> GetAsync(TKey id, TFilter filter = null);
    }
}