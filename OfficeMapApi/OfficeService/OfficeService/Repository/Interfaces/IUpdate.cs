using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
        public interface IUpdate<TEntity>
            where TEntity : class
        {
            Task UpdateAsync(TEntity entity);
        }
}

