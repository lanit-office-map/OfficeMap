using System;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface ICrudRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> CreateAsync(TEntity item);
        Task DeleteAsync(TKey id);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
