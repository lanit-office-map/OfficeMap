using System;
using OfficeService.Models;
using Common.Interfaces;

namespace OfficeService.Services.Interface
{
    public interface IOfficeService :
          IGet<OfficeResponse, Guid>,
          IDelete<Guid>,
          IFindAll<OfficeResponse>,
          ICreate<Office>,
          IUpdate<Office>
    {
    }
}