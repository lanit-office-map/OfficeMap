using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpaceService.Database.Entities;
using SpaceService.Repository.Interfaces;
using SpaceService.Filters;
using Microsoft.EntityFrameworkCore;

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

    public async Task<DbSpaceType> GetAsync(Guid spacetypeguid)
    {
      return await dbContext.SpaceTypes
        .FirstOrDefaultAsync(x => x.SpaceTypeGuid == spacetypeguid);
    }

    public async Task<DbSpaceType> UpdateAsync(DbSpaceType spacetype)
    {
      EntityEntry<DbSpaceType> result = dbContext.SpaceTypes.Update(spacetype);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task DeleteAsync(DbSpaceType spacetype)
    {
      dbContext.SpaceTypes.Remove(spacetype);
      await dbContext.SaveChangesAsync();
    }
    public async Task<DbSpaceType> CreateAsync(DbSpaceType spacetype)
    {
      EntityEntry<DbSpaceType> result = dbContext.SpaceTypes.Add(spacetype);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task<IEnumerable<DbSpaceType>> FindAllAsync()
    {
      return await dbContext.SpaceTypes.ToListAsync();
    }
  }
}
