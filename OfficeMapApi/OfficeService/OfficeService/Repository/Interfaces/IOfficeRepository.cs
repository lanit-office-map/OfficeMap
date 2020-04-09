using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Models;
using OfficeService.Repository.Filters;
using OfficeService.Database.Entities;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository:
        IGet<DbOffice, Guid>,
        IDelete<DbOffice>,
        IFind<DbOffice, OfficeFilter>,
        ICreate<DbOffice>,
        IUpdate<DbOffice>
    { 

    } 
}
