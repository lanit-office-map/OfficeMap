using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICreate<TEntity>
        where TEntity : class
    {
        Task<Response<TEntity>> CreateAsync(TEntity entity);
    }
}
