using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IQueryable<TEntity> GetAllAsync();
        IQueryable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(TKey guid);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
