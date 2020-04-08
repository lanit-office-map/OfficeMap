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

    public Task<DbOffice> UpdateOfficeAsync(DbOffice office)
        {
            dbContext.Entry(office).State = EntityState.Modified;
            return Task.FromResult(office);
        }

    public Task DeleteOfficeAsync(DbOffice office)
        {
            if (office != null)
            {
                dbContext.Offices.Remove(office);
            }
            return Task.CompletedTask;
        }
        
        // как вывести перечисление добавленных сущностей?
    public Task<DbOffice> CreateOfficeAsync(DbOffice office)
        {
            dbContext.Offices.Add(office);
            dbContext.SaveChanges();
            return Task.FromResult(office);
        }
    #endregion
  }
}
