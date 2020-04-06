using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeService.Database.Entities;

namespace OfficeService.Repository.Interfaces
{
  public interface IOfficeRepository
  {
    Task<IEnumerable<DbOffice>> GetOfficesAsync();
  }
}
