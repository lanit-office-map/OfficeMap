using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDelete<in TInput, TOutput>
        where TOutput : class
    {
        Task<TOutput> DeleteAsync(TInput input);
    }

    public interface IDelete<in TInput>
    {
      Task DeleteAsync(TInput input);
    }
}
