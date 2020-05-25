using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;
using AutoMapper;
using Common.Response;
using System.Net;

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

    public async Task<Response<IEnumerable<OfficeResponse>>> FindAllAsync()
    {
      var result = await officeRepository.FindAllAsync();

      var response = new Response<IEnumerable<OfficeResponse>>(automapper.Map<IEnumerable<OfficeResponse>>(result));
      return response;
    }

    public async Task<Response<OfficeResponse>> CreateAsync(Office office)
    {
      var result = await officeRepository.CreateAsync(automapper.Map<DbOffice>(office));

      var response = new Response<OfficeResponse>(automapper.Map<OfficeResponse>(result));
      return response;
    }

    public async Task<Response<OfficeResponse>> GetAsync(Guid officeguid)
    {
      var result = await officeRepository.GetAsync(officeguid);
      if (result == null)
      {
        return new Response<OfficeResponse>(
          HttpStatusCode.NotFound,
          $"Office with guid '{officeguid}' was not found");
      }

      var response = new Response<OfficeResponse>(automapper.Map<OfficeResponse>(result));
      return response;
    }

    public async Task DeleteAsync(Guid officeguid)
    {
      var source = await officeRepository.GetAsync(officeguid);
      if (source != null)
      {
        await officeRepository.DeleteAsync(source);
      }
    }

    public async Task<Response<OfficeResponse>> UpdateAsync(Office target)
    {
      var source = await officeRepository.GetAsync(target.OfficeGuid);
      if (source == null)
      {
        return new Response<OfficeResponse>(
          HttpStatusCode.NotFound,
          $"Office with guid '{target.OfficeGuid}' was not found"); ;
      }

      source.City = target.City;
      source.Building = target.Building;
      source.House = target.House;
      source.PhoneNumber = target.PhoneNumber;
      source.Street = target.Street;
      var result = await officeRepository.UpdateAsync(source);

      var response = new Response<OfficeResponse>(automapper.Map<OfficeResponse>(result));
      return response;
    }
    #endregion
  }
}

