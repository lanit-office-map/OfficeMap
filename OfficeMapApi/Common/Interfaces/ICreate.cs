using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICreate<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        Task<Response<TOutput>> CreateAsync(TInput entity);
    }
}
