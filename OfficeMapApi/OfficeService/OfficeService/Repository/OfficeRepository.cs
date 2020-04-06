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
    #endregion
  }
}
