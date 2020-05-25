using System;
using System.Collections.Generic;
using OfficeService.Models;
using Common.Interfaces;
using Common.Response;
using OfficeService.Filters;

namespace OfficeService.Services.Interface
{
    public interface IOfficeService :
          IGet<Guid, Response<OfficeResponse>>,
          IDelete<Guid>,
          IFindAll<Response<IEnumerable<OfficeResponse>>>,
          ICreate<Office, Response<OfficeResponse>>,
          IUpdate<Office, Response<OfficeResponse>>
    {
    }
}