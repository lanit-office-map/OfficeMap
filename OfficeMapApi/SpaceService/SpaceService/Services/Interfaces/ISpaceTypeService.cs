using System;
using System.Collections.Generic;
using SpaceService.Models;
using SpaceService.Filters;
using Common.Interfaces;
using Common.Response;

namespace SpaceService.Services.Interfaces
{
    public interface ISpaceTypeService :
      IGet<Guid, Response<SpaceTypeResponse>>,
      IDelete<Guid>,
      IFindAll<Response<IEnumerable<SpaceTypeResponse>>>,
      ICreate<SpaceType, Response<SpaceTypeResponse>>,
      IUpdate<SpaceType, Response<SpaceTypeResponse>>
  {

    }
}