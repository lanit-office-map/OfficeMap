using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
        public interface IUpdate <TEntity>
            where TEntity : class
        {
            Task<TEntity> UpdateAsync(TEntity entity);
        }
        public interface IUpdate <TSource, TTarget>
            where TSource : class
            where TTarget : class
        {
            Task<TTarget> UpdateAsync(TSource source, TTarget target);
        }
}

