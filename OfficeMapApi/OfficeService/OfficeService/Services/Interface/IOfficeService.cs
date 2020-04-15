using System;
using OfficeService.Models;
using OfficeService.Repository.Filters;
using OfficeService.Interfaces;

namespace OfficeService.Services.Interface
{
  public interface IOfficeService:
        IGet<Office, Guid>,
        IDelete<Guid>,
        IFind<Office, OfficeFilter>,
        ICreate<Office>,
        IUpdate<Office>
    {

    }
}