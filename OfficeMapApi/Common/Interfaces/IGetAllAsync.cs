using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGetAllAsync<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
    }
}
