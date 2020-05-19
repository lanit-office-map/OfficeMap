using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<in TInput, TOutput>
        where TOutput : class
    {
        Task<Response<TOutput>> DeleteAsync(TInput input);
    }
}

/*using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<in TInput>
    {
        Task<Response.Response> DeleteAsync(TInput input);
    }
}*/
