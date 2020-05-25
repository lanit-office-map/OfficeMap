using System;
using System.Collections.Generic;
using Common.Interfaces;
using WorkplaceService.Filters;
using WorkplaceService.Models;
using Common.Response;

namespace WorkplaceService.Services.Interfaces
{
  public interface IWorkplaceService :
    IGet<Guid, Response<WorkplaceResponse>>,
    IDelete<Guid>,
    IFindAll<WorkplaceFilter, Response<IEnumerable<WorkplaceResponse>>>,
    ICreate<Workplace, Response<WorkplaceResponse>>,
    IUpdate<Workplace, Response<WorkplaceResponse>>
  {
  }
}
