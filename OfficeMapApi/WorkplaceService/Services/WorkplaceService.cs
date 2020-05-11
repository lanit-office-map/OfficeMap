using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkplaceService.Database.Entities;
using WorkplaceService.Models;
using WorkplaceService.Repository.Interfaces;

namespace WorkplaceService.Services
{
    public class WorkplaceService : IWorkplaceService
    {
        #region private fields
        private readonly IWorkplaceRepository workplaceRepository;
        private readonly IMapper automapper;
        #endregion

        #region public methods
        public WorkplaceService(
          [FromServices] IWorkplaceRepository workplaceRepository,
          [FromServices] IMapper automapper)
        {
            this.workplaceRepository = workplaceRepository;
            this.automapper = automapper;
        }

        public Task<IEnumerable<WorkplaceResponse>> FindAllAsync()
        {
            var result = workplaceRepository.FindAllAsync(entity => entity.Obsolete == false);

            return Task.FromResult(automapper.Map<IEnumerable<WorkplaceResponse>>(result));
        }

        public Task<Workplace> CreateAsync(Workplace entity)
        {
            var result = workplaceRepository.CreateAsync(automapper.Map<DbWorkplace>(entity)).Result;

            return Task.FromResult(automapper.Map<Workplace>(result));
        }

        public Task<WorkplaceResponse> GetAsync(Guid guid)
        {
            var result = workplaceRepository.GetAsync(guid).Result;

            return Task.FromResult(automapper.Map<WorkplaceResponse>(result));
        }

        public Task DeleteAsync(Guid guid)
        {
            var source = workplaceRepository.GetAsync(guid).Result;
            if (source != null)
            {
                workplaceRepository.DeleteAsync(source);
            }

            return Task.CompletedTask;
        }

        public Task<Workplace> UpdateAsync(Workplace target)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
