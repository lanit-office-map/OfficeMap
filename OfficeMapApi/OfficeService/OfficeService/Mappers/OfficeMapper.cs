using System;
using OfficeService.Database.Entities;
using OfficeService.Mappers.Interfaces;
using OfficeService.Models;

namespace OfficeService.Mappers
{
  public class OfficeMapper : IOfficeMapper
  {
    public Office Map(DbOffice model)
    {
      if (model == null)
      {
        return null;
      }

      return new Office
      {
        OfficeGuid = model.OfficeGuid,
        Building = model.Building,
        City = model.City,
        House = model.House,
        PhoneNumber = model.PhoneNumber,
        Street = model.Street
      };
    }
  }
}
