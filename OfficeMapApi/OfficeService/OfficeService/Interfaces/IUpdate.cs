using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
        public interface IUpdate <TResponse, TEntity>
            where TResponse : class
        {
            Task<TResponse> UpdateAsync(TEntity entity);
        }
}

