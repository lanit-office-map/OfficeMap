using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Filters;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;
using AutoMapper;

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

    public Task<IEnumerable<OfficeResponse>> FindAsync(OfficeFilter filter)
    {
      var result = officeRepository.FindAsync(filter).Result;

      return  Task.FromResult(automapper.Map<IEnumerable<OfficeResponse>>(result));
    }
    
    public Task<Office> CreateAsync(Office office)
        {
            var result = officeRepository.CreateAsync(automapper.Map<DbOffice>(office)).Result;
            return Task.FromResult(automapper.Map<Office>(result));
        }
        public Task<OfficeResponse> GetAsync(Guid officeguid)
        { 
            var result = officeRepository.GetAsync(officeguid).Result;

            return Task.FromResult(automapper.Map<OfficeResponse>(result));
        }

    public Task DeleteAsync(Guid officeguid)
        {
            var source = officeRepository.GetAsync(officeguid).Result;
            if (source != null)
            {
                officeRepository.DeleteAsync(source);
            }
            return Task.CompletedTask;
        }

    public Task<OfficeResponse> UpdateAsync(Office target)
        {
            var source = officeRepository.GetAsync(target.OfficeGuid).Result;
            if (source == null)
            {
                // TODO создать ошибку для Not Found
                throw new NotImplementedException();
            }
            source.City = target.City;
            source.Building = target.Building;
            source.House = target.House;
            source.PhoneNumber = target.PhoneNumber;
            source.Street = target.Street;
            var result = officeRepository.UpdateAsync(source).Result;
            return Task.FromResult(automapper.Map<OfficeResponse>(result));
        }
    #endregion

  }
}
