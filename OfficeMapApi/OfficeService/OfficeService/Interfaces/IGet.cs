using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeService.Interfaces
{ 
    public interface IGet<TEntity, in TKey>
        where TEntity : class
    {
        Task<TEntity> GetAsync(TKey id);
    }
}
