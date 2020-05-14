using Common.Repositories;
using System;
using System.Collections.Generic;
using WorkplaceService.Database.Entities;

namespace WorkplaceService.Repository.Interfaces
{
    public interface IWorkplaceRepository : ICrudRepository<DbWorkplace, Guid>
    {
        public IEnumerable<DbWorkplace> GetAllAsync(int OfficeId);
    }
}
