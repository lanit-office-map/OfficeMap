using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
        public interface IUpdate <TEntity>
            where TEntity : class
        {
            Task<TEntity> UpdateAsync(TEntity entity);
        }
}

