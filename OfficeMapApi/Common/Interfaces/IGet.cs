using Common.Response;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGet<in TKey, TOutput>
        where TOutput : class
    {
        Task<Response<TOutput>> GetAsync(TKey key);
    }
}

/*using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IGet<in TKey>
    {
        Task<Response.Response> GetAsync(TKey key);
    }
}*/
