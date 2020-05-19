using Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAll<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        Task<Response<IEnumerable<TOutput>>> FindAllAsync(TInput input = null);
    }
}

/*using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAll<TInput>
        where TInput : class
    {
        Task<Response.Response> FindAllAsync(TInput input = null);
    }
}*/
