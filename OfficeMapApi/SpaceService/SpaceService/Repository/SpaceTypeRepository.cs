﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpaceService.Database.Entities;
using SpaceService.Repository.Interfaces;
using SpaceService.Filters;

namespace SpaceService.Repository
{
    public class SpaceTypeRepository : ISpaceTypeRepository
    {
        private readonly SpaceServiceDbContext dbContext;

        public SpaceTypeRepository(
          [FromServices] SpaceServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<DbSpaceType> GetAsync(Guid spacetypeguid)
        {
            return Task.FromResult(dbContext.SpaceTypes.FirstOrDefault(x =>
            x.SpaceTypeGuid == spacetypeguid && x.Obsolete == false));
        }

        public Task<DbSpaceType> UpdateAsync(DbSpaceType spacetype)
        {
            EntityEntry<DbSpaceType> result = dbContext.SpaceTypes.Update(spacetype);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }

        public Task DeleteAsync(DbSpaceType spacetype)
        {
            dbContext.SpaceTypes.Remove(spacetype);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
        public Task<DbSpaceType> CreateAsync(DbSpaceType spacetype)
        {
            EntityEntry<DbSpaceType> result = dbContext.SpaceTypes.Add(spacetype);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }

        public Task<IEnumerable<DbSpaceType>> FindAsync(SpaceTypeFilter filter = null)
        {
            if (filter != null)
            {
                throw new NotImplementedException();
            }
            return Task.FromResult(dbContext.SpaceTypes.Where(x => x.Obsolete == false).AsEnumerable());
        }
    }
}