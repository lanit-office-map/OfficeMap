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
        Task<DbOffice> CreateOfficeAsync(DbOffice office);
        Task DeleteOfficeAsync(DbOffice office);
        Task<DbOffice> UpdateOfficeAsync(DbOffice office);
        Task<DbOffice> GetOfficeAsync(Guid officeguid);
    } 
}
