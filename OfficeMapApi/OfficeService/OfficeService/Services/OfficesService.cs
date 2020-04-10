using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;
using AutoMapper;
using OfficeService.Repository.Filters;

namespace OfficeService.Services
{
  public class OfficesService : IOfficeService
  {
    #region private fields
    private readonly IOfficeRepository officeRepository;
    private readonly IMapper automapper;
    #endregion

     

    #region public methods
    public OfficesService(
      [FromServices] IOfficeRepository officeRepository,
      [FromServices] IMapper automapper)
    {
      this.officeRepository = officeRepository;
      this.automapper = automapper;
    }

    public Task<IEnumerable<Office>> FindAsync(OfficeFilter filter)
    {
      var result = officeRepository.FindAsync(filter).Result;

      return  Task.FromResult(automapper.Map<IEnumerable<Office>>(result));
    }
    
    public Task<Office> CreateAsync(Office office)
        {
            var result = officeRepository.CreateAsync(automapper.Map<DbOffice>(office)).Result;
            return Task.FromResult(automapper.Map<Office>(result));
        }

    public Task<Office> GetAsync(Guid officeguid)
        { 
            var result = officeRepository.GetAsync(officeguid).Result;

            return Task.FromResult(automapper.Map<Office>(result));
        }

    public Task DeleteAsync(Office office)
        {
            officeRepository.DeleteAsync(automapper.Map<DbOffice>(office));

            return Task.CompletedTask;
        }

    public Task<Office> UpdateAsync(Office source, Office target)
        {
            // TODO добавить логику присваивания из Target в Source
            var result = officeRepository.UpdateAsync(automapper.Map<DbOffice>(source));
            return Task.FromResult(automapper.Map<Office>(result));
        }
    #endregion

  }
}
