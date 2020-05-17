using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUpdate<TEntity>
        where TEntity : class
    {
        Task<Response<TEntity>> UpdateAsync(TEntity entity);
    }
}
