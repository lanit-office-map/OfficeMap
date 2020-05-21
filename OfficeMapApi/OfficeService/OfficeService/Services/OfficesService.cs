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

        private static class Responses<T>
            where T : class
        {
            public static readonly Response<T> OfficeNotFounded =
                new Response<T>(HttpStatusCode.NotFound, $"Office by officeGuid not founded");
        }
        #endregion

        #region public methods
        public OfficesService(
          [FromServices] IOfficeRepository officeRepository,
          [FromServices] IMapper automapper)
        {
            this.officeRepository = officeRepository;
            this.automapper = automapper;
        }

        public Task<Response<IEnumerable<OfficeResponse>>> FindAllAsync()
        {
            var result = officeRepository.FindAllAsync(office => office.Obsolete == false);

            var response = new Response<IEnumerable<OfficeResponse>>(automapper.Map<IEnumerable<OfficeResponse>>(result));
            return Task.FromResult(response);
        }

        public Task<Response<Office>> CreateAsync(Office office)
        {
            var result = officeRepository.CreateAsync(automapper.Map<DbOffice>(office)).Result;

            var response = new Response<Office>(automapper.Map<Office>(result));
            return Task.FromResult(response);
        }
        
        public Task<Response<OfficeResponse>> GetAsync(Guid officeguid)
        {
            var result = officeRepository.GetAsync(officeguid).Result;
            if (result == null)
            {
                return Task.FromResult(Responses<OfficeResponse>.OfficeNotFounded);
            }

            var response = new Response<OfficeResponse>(automapper.Map<OfficeResponse>(result));
            return Task.FromResult(response);
        }

        public Task<Response<OfficeResponse>> DeleteAsync(Guid officeguid)
        {
            var source = officeRepository.GetAsync(officeguid).Result;
            if (source != null)
            {
                officeRepository.DeleteAsync(source);
            }

            var response = new Response<OfficeResponse>();
            return Task.FromResult(response);
        }

        public Task<Response<Office>> UpdateAsync(Office target)
        {
            var source = officeRepository.GetAsync(target.Guid).Result;
            if (source == null)
            {
                return Task.FromResult(Responses<Office>.OfficeNotFounded);
            }

            source.City = target.City;
            source.Building = target.Building;
            source.House = target.House;
            source.PhoneNumber = target.PhoneNumber;
            source.Street = target.Street;
            var result = officeRepository.UpdateAsync(source).Result;

            var response = new Response<Office>(automapper.Map<Office>(result));
            return Task.FromResult(response);
        }
        #endregion
    }
}

