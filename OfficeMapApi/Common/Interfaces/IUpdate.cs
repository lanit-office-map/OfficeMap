using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUpdate<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        Task<Response<TOutput>> UpdateAsync(TInput input);
    }
}

/*using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IUpdate<TInput>
        where TInput : class
    {
        Task<Response.Response> UpdateAsync(TInput input);
    }
}*/
