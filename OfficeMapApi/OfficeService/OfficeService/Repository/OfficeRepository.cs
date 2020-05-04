using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database;
using OfficeService.Database.Entities;
using OfficeService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OfficeService.Repository.Filters;

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

        public Task<DbOffice> GetAsync(Guid officeguid)
        {
            return Task.FromResult(dbContext.Offices.FirstOrDefault(x => 
            x.OfficeGuid == officeguid && x.Obsolete == false) );
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

    public Task<IEnumerable<DbOffice>> FindAsync(OfficeFilter filter = null)
        {
            if (filter != null)
            {
                throw new NotImplementedException();
            }
            return Task.FromResult(dbContext.Offices.Where(x => x.Obsolete == false).AsEnumerable());
        }
    #endregion
  }
}
 