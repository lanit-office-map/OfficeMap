using System;
using OfficeService.Models;
using OfficeService.Repository.Filters;
using OfficeService.Interfaces;

namespace OfficeService.Services.Interface
{
  public interface IOfficeService:
        IGet<OfficeResponse, Guid>,
        IDelete<Guid>,
        IFind<OfficeResponse, OfficeFilter>,
        ICreate<Office>,
        IUpdate<OfficeResponse, Office>
    {

    }
}