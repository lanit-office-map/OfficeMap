using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFind<TEntity>
    {
        Task<IEnumerable<TEntity>> FindAsync();
    }
}
