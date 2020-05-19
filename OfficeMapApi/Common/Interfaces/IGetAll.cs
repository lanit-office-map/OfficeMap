using Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGetAll<TOutput>
        where TOutput : class
    {
        Task<Response<IEnumerable<TOutput>>> GetAllAsync();
    }

    public interface IGetAll<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        Task<Response<TOutput>> GetAllAsync(TInput input);
    }
}
