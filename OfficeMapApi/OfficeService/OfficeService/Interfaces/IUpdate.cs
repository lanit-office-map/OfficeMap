using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
        public interface IUpdate <TEntity>
            where TEntity : class
        {
            Task<TEntity> UpdateAsync(TEntity entity);
        }
}

