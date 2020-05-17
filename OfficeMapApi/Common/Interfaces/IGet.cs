using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGet<TEntity, in TKey>
        where TEntity : class
    {
        Task<Response<TEntity>> GetAsync(TKey id);
    }
}
