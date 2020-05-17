using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<TEntity>
        where TEntity : class
    {
        Task<Response<TEntity>> DeleteAsync(TEntity entity);
    }
}
