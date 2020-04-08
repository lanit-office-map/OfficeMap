using System;
using System.Collections.Generic;
using System.Linq;
using System.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeService.Database;
using OfficeService.Database.Entities;
using OfficeService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OfficeService.Repository
{
  public class OfficeRepository : IOfficeRepository
  {
    #region private fields
    private readonly OfficeServiceDbContext dbContext;
    #endregion

    #region public methods
    public OfficeRepository(
      [FromServices] OfficeServiceDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public Task<IEnumerable<DbOffice>> GetOfficesAsync()
    {
            return Task.FromResult(dbContext.Offices.AsEnumerable());
    }

    public Task<DbOffice> GetAsync(Guid officeguid)
        {
            return Task.FromResult(dbContext.Offices.Find(officeguid));
        }

    public Task<DbOffice> UpdateAsync(DbOffice office)
        {
            EntityEntry<DbOffice> result = dbContext.Offices.Update(office);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }

    public Task DeleteAsync(DbOffice office)
        {
                dbContext.Offices.Remove(office);
                dbContext.SaveChanges();
                return Task.CompletedTask;
        }
    public Task<DbOffice> CreateAsync(DbOffice office)
        {
            EntityEntry<DbOffice> result = dbContext.Offices.Add(office);
            dbContext.SaveChanges();
            return Task.FromResult(result.Entity);
        }
    #endregion
  }
}
 