using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    private readonly OfficeServiceDbContext dbContext;
    public OfficeRepository([FromServices] OfficeServiceDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public async Task<DbOffice> CreateAsync(DbOffice entity)
    {
      var result = dbContext.Offices.Add(entity);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }

    public async Task DeleteAsync(DbOffice entity)
    {
      dbContext.Offices.Remove(entity);
      await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<DbOffice>> FindAllAsync()
    {
      return await dbContext.Offices.ToListAsync();
    }

    public async Task<DbOffice> GetAsync(Guid key)
    {
      return await dbContext.Offices.FirstOrDefaultAsync(x => x.OfficeGuid == key);
    }

    public async Task<DbOffice> UpdateAsync(DbOffice entity)
    {
      var result = dbContext.Offices.Update(entity);
      await dbContext.SaveChangesAsync();
      return result.Entity;
    }
  }
}
