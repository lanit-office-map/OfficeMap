using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WorkplaceService.Database;
using WorkplaceService.Database.Entities;
using WorkplaceService.Repository.Interfaces;

namespace WorkplaceService.Repository
{
    public class WorkplaceRepository : CrudRepository<DbWorkplace, Guid>, IWorkplaceRepository
    {
        public WorkplaceRepository([FromServices] WorkplaceServiceDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<DbWorkplace> GetBySpaceGuidAsync()
        {
            throw new NotImplementedException();
        }
    }
}
