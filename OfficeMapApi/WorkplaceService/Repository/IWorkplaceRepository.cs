using Common.Repositories;
using System;
using WorkplaceService.Database.Entities;

namespace WorkplaceService.Repository
{
    public interface IWorkplaceRepository : ICrudRepository<DbWorkplace, Guid>
    {
    }
}
