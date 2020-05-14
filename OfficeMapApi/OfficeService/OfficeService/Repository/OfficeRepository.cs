using System;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database;
using OfficeService.Database.Entities;
using OfficeService.Repository.Interfaces;
using Common.Repositories;

namespace OfficeService.Repository
{
    public class OfficeRepository : CrudRepository<DbOffice, Guid>, IOfficeRepository
    {
        public OfficeRepository([FromServices] OfficeServiceDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
