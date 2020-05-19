using System;
using OfficeService.Models;
using Common.Interfaces;
using OfficeService.Filters;

namespace OfficeService.Services.Interface
{
    public interface IOfficeService :
          IGet<Guid, OfficeResponse>,
          IDelete<Guid, OfficeResponse>,
          IFindAll<OfficeResponse>,
          ICreate<Office, Office>,
          IUpdate<Office, Office>
    {
    }
}