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

namespace SpaceService.Repository
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly SpaceServiceDbContext dbContext;

        private void LoadNestedSpaces(DbSpace parent)
        {
            foreach (var space in parent.Spaces)
            {
                dbContext.Entry(space).Collection(s => s.Spaces).Load();
                dbContext.Entry(space).Reference(s => s.MapFile).Load();
                dbContext.Entry(space).Reference(s => s.SpaceType).Load();
                LoadNestedSpaces(space);    
            }
        }

        public SpaceRepository(
            [FromServices] SpaceServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public Task<DbSpace> GetAsync(Guid spaceguid, SpaceFilter filter = null)
        {
            DbSpace result = null;
            if (filter != null)
            {
                result = dbContext.Spaces
                    .Include(s => s.Spaces)
                    .Include(s => s.MapFile)
                    .Include(s => s.SpaceType)
                    .Single(s =>
                            s.SpaceGuid == spaceguid && s.OfficeId == filter.OfficeId);
                LoadNestedSpaces(result);
                return Task.FromResult(result);
            }
            result = dbContext.Spaces
                    .Include(s => s.Spaces)
                    .Include(s => s.MapFile)
                    .Include(s => s.SpaceType)
                    .Single(s =>
                            s.SpaceGuid == spaceguid);
            LoadNestedSpaces(result);
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
            IEnumerable<DbSpace> result = null;
            if (filter != null)
            {
                 result = dbContext.Spaces
                .Include(s => s.Spaces)
                .Include(s => s.MapFile)
                .Include(s => s.SpaceType)
                .Where(s =>
                        s.OfficeId == filter.OfficeId && s.Parent == null)
                .ToList();
                foreach (var space in result)
                {
                    LoadNestedSpaces(space);
                }
                return Task.FromResult(result);
            }          
            result = dbContext.Spaces
                .Include(s => s.Spaces)
                .Include(s => s.MapFile)
                .Include(s => s.SpaceType)
                .Where  (s => s.Parent == null)
            .ToList();
            foreach (var space in result)
            {
                LoadNestedSpaces(space);
            }
            return Task.FromResult(result);
        }

    }
}
