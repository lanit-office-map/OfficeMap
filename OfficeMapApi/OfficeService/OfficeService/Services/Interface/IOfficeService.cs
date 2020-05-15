using System;
using OfficeService.Models;
using Common.Interfaces;
using OfficeService.Filters;

namespace OfficeService.Services.Interface
{
    public interface IOfficeService :
          IGet<OfficeResponse, Guid>,
          IDelete<Guid>,
          IFindAll<OfficeResponse, OfficeFilter>,
          ICreate<Office>,
          IUpdate<Office>
    {
    }
}