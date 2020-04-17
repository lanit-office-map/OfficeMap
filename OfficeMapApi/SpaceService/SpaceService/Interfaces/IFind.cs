using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface IFind<TEntity, TFilter>
            where TFilter : class
    {
        Task<IEnumerable<TEntity>> FindAsync(TFilter filter = null);
    }
}