using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;
using AutoMapper;

namespace OfficeService.Services
{
    public class OfficesService : IOfficeService
    {
        private readonly IOfficeRepository officeRepository;
        private readonly IMapper automapper;

        public OfficesService(
          [FromServices] IOfficeRepository officeRepository,
          [FromServices] IMapper automapper)
        {
            this.officeRepository = officeRepository;
            this.automapper = automapper;
        }

        public Task<IEnumerable<Office>> FindAllAsync()
        {
            var result = officeRepository.FindAllAsync(office => office.Obsolete == false);

            return Task.FromResult(automapper.Map<IEnumerable<Office>>(result));
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

        public Task DeleteAsync(Guid officeguid)
        {
            var source = officeRepository.GetAsync(officeguid).Result;
            if (source != null)
            {
                officeRepository.DeleteAsync(source);
            }

            return Task.CompletedTask;
        }

        public Task<Office> UpdateAsync(Office target)
        {
            var source = officeRepository.GetAsync(target.Guid).Result;
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
            return Task.FromResult(automapper.Map<Office>(result));
        }
    }
}
