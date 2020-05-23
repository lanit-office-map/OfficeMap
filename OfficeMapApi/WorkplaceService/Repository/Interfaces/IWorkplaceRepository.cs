using System;
using System.Collections.Generic;
using Common.Interfaces;
using WorkplaceService.Database.Entities;
using WorkplaceService.Filters;

namespace WorkplaceService.Repository.Interfaces
{
  public interface IWorkplaceRepository :
    IGet<Guid, DbWorkplace>,
    IFindAll<WorkplaceFilter, IEnumerable<DbWorkplace>>,
    IUpdate<DbWorkplace, DbWorkplace>,
    ICreate<DbWorkplace, DbWorkplace>,
    IDelete<DbWorkplace>
  {
  }
}
