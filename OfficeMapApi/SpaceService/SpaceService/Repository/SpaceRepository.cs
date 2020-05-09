using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using SpaceService.Database.Entities;
using SpaceService.Filters;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

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
            var result = dbContext.Spaces
                .IncludeFilter(s => s.InverseParent.Where(i => i.Obsolete == false))
              //  .Include(s => s.Offices)
              //  .Include(s => s.MapFiles)
              //  .Include(s => s.SpaceTypes)
                .FirstOrDefault(x =>
                                x.SpaceGuid == spaceguid &&
                                x.Obsolete == false);
            return Task.FromResult(result);
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
                var result = dbContext.Spaces
                    .IncludeFilter(s => s.InverseParent.Where(i => i.Obsolete == false))
                    .IncludeFilter(s => s.SpaceTypes)
                    .IncludeFilter(s => s.MapFiles)
                .Where(s => s.Obsolete == false && s.OfficeId == filter.OfficeId)
                .AsEnumerable();
                return Task.FromResult(result);
            }          
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
