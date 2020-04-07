using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Filters;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository :
    
        ICreate<OfficeRequest>,
        IDelete<Office>,
        IGet<Office, OfficeFilter>,
        IUpdate<OfficeRequest>
        
    { 
        Task<IEnumerable<DbOffice>> GetOfficesAsync();
    }
}
