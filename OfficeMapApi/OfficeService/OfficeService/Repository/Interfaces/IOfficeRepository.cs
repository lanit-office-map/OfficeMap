using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Database.Entities;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository       
    { 
        Task<IEnumerable<DbOffice>> GetOfficesAsync();
        Task<DbOffice> CreateAsync(DbOffice office);
        Task DeleteAsync(DbOffice office);
        Task<DbOffice> UpdateAsync(DbOffice office);
        Task<DbOffice> GetAsync(Guid officeguid);
        Task<IEnumerable> FindAsync(Guid officeguid);
    } 
}
