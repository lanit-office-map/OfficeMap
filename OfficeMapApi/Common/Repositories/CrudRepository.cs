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

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            var entity = await Context.AddAsync(item);
            await Context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task DeleteAsync(TKey id)
        {
            await Context.Set<TEntity>().Where(entity => entity.Guid.Equals(id)).DeleteAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updatedEntity = Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
            return updatedEntity.Entity;
        }
    }
}
