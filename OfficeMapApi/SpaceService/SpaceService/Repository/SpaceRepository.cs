using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpaceService.Database.Entities;
using SpaceService.Filters;
using SpaceService.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Repository
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly SpaceServiceDbContext dbContext;

        public SpaceRepository(
            [FromServices] SpaceServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<DbSpace> GetAsync(Guid spaceguid)
        {
            return Task.FromResult(dbContext.Spaces.FirstOrDefault(x =>
            x.SpaceGuid == spaceguid && x.Obsolete == false));
        }

        public Task<DbSpace> UpdateAsync(DbSpace space)
        {
            EntityEntry<DbSpace> result = dbContext.Spaces.Update(space);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }

        public Task DeleteAsync(DbSpace space)
        {
            dbContext.Spaces.Remove(space);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
        public Task<DbSpace> CreateAsync(DbSpace space)
        {
            EntityEntry<DbSpace> result = dbContext.Spaces.Add(space);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }

        public Task<IEnumerable<DbSpace>> FindAsync(SpaceFilter filter = null)
        {
            if (filter != null)
            {
                return Task.FromResult(dbContext.Spaces.Where(x => x.Obsolete == false && x.OfficeId == filter.OfficeId).AsEnumerable());
            }
            return Task.FromResult(dbContext.Spaces.Where(x => x.Obsolete == false).AsEnumerable());
        }

    }
}
