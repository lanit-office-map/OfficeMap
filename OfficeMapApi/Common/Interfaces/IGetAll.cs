using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGetAll<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
