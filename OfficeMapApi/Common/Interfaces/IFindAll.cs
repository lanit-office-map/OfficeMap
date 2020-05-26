using System.Collections;
using Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAll<TOutput>
        where TOutput : class
    {
        Task<TOutput> FindAllAsync();
    }

    public interface IFindAll<TInput, TOutput>
        where TInput : class
        where TOutput : class
  {
        Task<TOutput> FindAllAsync(TInput input);
    }
}
