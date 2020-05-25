using SpaceService.Database.Entities;
using System;
using System.Collections.Generic;
using Common.Interfaces;
using SpaceService.Filters;

namespace SpaceService.Repository.Interfaces
{
  public interface ISpaceRepository :
    IGet<Guid, DbSpace>,
    IFindAll<SpaceFilter, IEnumerable<DbSpace>>,
    IUpdate<DbSpace, DbSpace>,
    ICreate<DbSpace, DbSpace>,
    IDelete<DbSpace>
  {
  }
}
