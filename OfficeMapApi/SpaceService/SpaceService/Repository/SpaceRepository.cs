using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using SpaceService.Database.Entities;
using SpaceService.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceService.Repository.Interfaces;

namespace SpaceService.Repository
{
  public class SpaceRepository : ISpaceRepository
  {
    private readonly SpaceServiceDbContext dbContext;

    private async Task LoadNestedSpaces(DbSpace parent)
    {
      if (parent == null)
      {
        return;
      }

      var result = Parallel.ForEach(
        parent.Spaces,
        async (space) =>
        {
          await dbContext.Entry(space).Collection(s => s.Spaces).LoadAsync();
          await dbContext.Entry(space).Reference(s => s.MapFile).LoadAsync();
          await dbContext.Entry(space).Reference(s => s.SpaceType).LoadAsync();
          await LoadNestedSpaces(space);
        });
    }

    public SpaceRepository(
        [FromServices] SpaceServiceDbContext dbContext)
    {
      this.dbContext = dbContext;
    }


    public async Task<DbSpace> GetAsync(Guid spaceguid)
    {
      var result = await dbContext.Spaces
                .Include(s => s.Spaces)
                .Include(s => s.MapFile)
                .Include(s => s.SpaceType)
                .FirstOrDefaultAsync(s =>
                        s.SpaceGuid == spaceguid);
      await LoadNestedSpaces(result);
      return result;
    }

    public async Task<DbSpace> UpdateAsync(DbSpace space)
    {
      EntityEntry<DbSpace> result = dbContext.Spaces.Update(space);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task DeleteAsync(DbSpace space)
    {
      dbContext.Spaces.Remove(space);
      await dbContext.SaveChangesAsync();
    }
    public async Task<DbSpace> CreateAsync(DbSpace space)
    {
      EntityEntry<DbSpace> result = dbContext.Spaces.Add(space);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task<IEnumerable<DbSpace>> FindAllAsync(SpaceFilter filter = null)
    {
      var result = await dbContext.Spaces
          .Include(s => s.Spaces)
          .Include(s => s.MapFile)
          .Include(s => s.SpaceType)
          .Where(s => s.Parent == null &&
                      (filter == null ||
                      s.OfficeId == filter.OfficeId))
      .ToListAsync();

      Parallel.ForEach(
        result,
        async (space) => await LoadNestedSpaces(space));

      return result;
    }

  }
}
