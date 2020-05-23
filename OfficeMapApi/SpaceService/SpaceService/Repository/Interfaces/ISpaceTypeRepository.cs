using SpaceService.Database.Entities;
using SpaceService.Filters;
using System;
using System.Collections.Generic;
using Common.Interfaces;

namespace SpaceService.Repository.Interfaces
{
  public interface ISpaceTypeRepository :
    IGet<Guid, DbSpaceType>,
    IFindAll<IEnumerable<DbSpaceType>>,
    IUpdate<DbSpaceType, DbSpaceType>,
    ICreate<DbSpaceType, DbSpaceType>,
    IDelete<DbSpaceType>
  {

  }
}