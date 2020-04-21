using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface IGet<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);
    }
}