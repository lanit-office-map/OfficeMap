using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Database.Entities;
using SpaceService.Repository.Interfaces;
using SpaceService.Repository.Filters;

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
