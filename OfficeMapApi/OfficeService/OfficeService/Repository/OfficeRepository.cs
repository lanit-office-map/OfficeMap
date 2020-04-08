using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeService.Database;
using OfficeService.Database.Entities;
using OfficeService.Repository.Interfaces;

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

    public Task<DbOffice> GetOfficeAsync(Guid officeguid)
        {
            return Task.FromResult(dbContext.Offices.Find(officeguid));
        }

    public Task<DbOffice> PutOfficeAsync(Guid officeguid)
        {
            DbOffice office = dbContext.Offices.Find(officeguid);
            dbContext.Entry(office).State = EntityState.Modified;
            return Task.FromResult(dbContext.Offices.Find(officeguid));
        }

    public Task<DbOffice> DeleteOfficeAsync(Guid officeguid)
        {
            DbOffice office = dbContext.Offices.Find(officeguid);
            if (office != null)
            {
                dbContext.Offices.Remove(office);
            }
            return Task.FromResult(dbContext.Offices.Find(officeguid));
        }
        
        // как вывести перечисление добавленных сущностей?
    public Task<IEnumerable<DbOffice>> PostOfficesAsync(DbOffice offices)
        {
            dbContext.Offices.Add(offices);
            return Task.FromResult(dbContext.Offices.AsEnumerable());
        }
    #endregion
  }
}
