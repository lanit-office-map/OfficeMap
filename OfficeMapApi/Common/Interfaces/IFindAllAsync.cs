using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAllAsync<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAll();
    }
}
