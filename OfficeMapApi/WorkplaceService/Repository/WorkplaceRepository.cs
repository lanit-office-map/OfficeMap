using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkplaceService.Database;
using WorkplaceService.Database.Entities;
using WorkplaceService.Filters;
using WorkplaceService.Repository.Interfaces;

namespace WorkplaceService.Repository
{
  public class WorkplaceRepository : IWorkplaceRepository
  {
    private WorkplaceServiceDbContext dbContext;
    public WorkplaceRepository([FromServices] WorkplaceServiceDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public async Task<DbWorkplace> CreateAsync(DbWorkplace entity)
    {
      var result = dbContext.Workplaces.Add(entity);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task DeleteAsync(DbWorkplace input)
    {
      dbContext.Workplaces.Remove(input);
      await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<DbWorkplace>> FindAllAsync(WorkplaceFilter filter)
    {
      return await dbContext.Workplaces
        .Include(w => w.Map)
        .Where(w => filter == null ||
                   (filter.SpaceId.HasValue && w.SpaceId == filter.SpaceId) ||
                   (filter.EmployeeId.HasValue && w.EmployeeId == filter.EmployeeId))
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<DbWorkplace> GetAsync(Guid key)
    {
      return await dbContext.Workplaces
        .Include(w => w.Map)
        .FirstOrDefaultAsync(
        w => w.WorkplaceGuid == key);
    }

    public async Task<DbWorkplace> UpdateAsync(DbWorkplace input)
    {
      var result = dbContext.Workplaces.Update(input);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }
  }
}
