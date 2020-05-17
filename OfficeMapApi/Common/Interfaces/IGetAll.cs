using Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGetAll<TEntity>
        where TEntity : class
    {
        Task<Response<IEnumerable<TEntity>>> GetAllAsync();
    }
}
