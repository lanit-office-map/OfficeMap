using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Repository.Interfaces;
using System;
using WorkplaceService.Database;

namespace WorkplaceService.Repository
{
    public class WorkplaceRepository : CrudRepository<DbOffice, Guid>, IOfficeRepository
    {
        public WorkplaceRepository([FromServices] WorkplaceServiceDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
