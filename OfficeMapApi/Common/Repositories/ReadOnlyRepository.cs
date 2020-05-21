using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        protected readonly DbContext Context;

        public ReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Returns entities that the change tracker will not track
        /// </summary>
        public virtual IQueryable<TEntity> GetAllAsync()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Returns entities that the change tracker will not track
        /// </summary>
        public virtual IQueryable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public virtual async Task<TEntity> GetAsync(TKey guid)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Guid.Equals(guid));
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }
    }
}
