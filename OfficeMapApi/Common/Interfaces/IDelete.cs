using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<TEntity, in TKey>
        where TEntity : class
    {
        Task<Response<TEntity>> DeleteAsync(TKey entity);
    }
}
