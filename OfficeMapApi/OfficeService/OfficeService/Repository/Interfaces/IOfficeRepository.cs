using System;
using OfficeService.Database.Entities;
using Common.Repositories;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository : ICrudRepository<DbOffice, Guid>
    {
    }
}
