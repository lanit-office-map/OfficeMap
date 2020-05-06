using System;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface ICrudRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TKey guid);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
