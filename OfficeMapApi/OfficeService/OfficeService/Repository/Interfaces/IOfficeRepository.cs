using System;
using OfficeService.Repository.Filters;
using OfficeService.Database.Entities;
using OfficeService.Interfaces;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository:
        IGet<DbOffice, Guid>,
        IDelete<DbOffice>,
        IFind<DbOffice, OfficeFilter>,
        ICreate<DbOffice>,
        IUpdate<DbOffice, DbOffice>
    { 

    } 
}