using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{ 
    public interface IGet<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);
    }
}
