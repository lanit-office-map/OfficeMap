using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Common.Repositories
{
    public class CrudRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, ICrudRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public CrudRepository(DbContext context)
            : base(context)
        {
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entityEntry = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await DeleteAsync(entity.Guid);
        }

        public virtual async Task DeleteAsync(TKey guid)
        {
            await Context.Set<TEntity>().Where(entity => entity.Guid.Equals(guid)).DeleteAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity item)
        {
            var entityEntry = Context.Set<TEntity>().Update(item);
            await Context.SaveChangesAsync();
            return entityEntry.Entity;
        }
    }
}
