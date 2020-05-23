
using SpaceService.Models;
using SpaceService.Filters;
using System;
using System.Collections.Generic;
using Common.Interfaces;
using Common.Response;

namespace SpaceService.Services.Interfaces
{
  public interface ISpacesService :
    IGet<Guid, Response<SpaceResponse>>,
    IDelete<Guid>,
    IFindAll<SpaceFilter, Response<IEnumerable<SpaceResponse>>>,
    ICreate<Space, Response<SpaceResponse>>,
    IUpdate<Space, Response<SpaceResponse>>
  {

  }
}
