using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Filters;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository       
    { 
        Task<IEnumerable<DbOffice>> GetOfficesAsync();
        Task<IEnumerable<DbOffice>> PostOfficesAsync(DbOffice offices);
        Task<DbOffice> DeleteOfficeAsync(Guid officeguid);
        Task<DbOffice> PutOfficeAsync(Guid officeguid);
        Task<DbOffice> GetOfficeAsync(Guid officeguid);
    } 
}
