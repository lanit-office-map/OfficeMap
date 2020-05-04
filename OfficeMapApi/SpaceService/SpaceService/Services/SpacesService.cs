using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Database.Entities;
using SpaceService.Filters;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using SpaceService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceService.Services
{
    public class SpacesService : ISpacesService
    {
        private readonly ISpaceRepository spaceRepository;
        private readonly IMapper automapper;

        public SpacesService(
          [FromServices] ISpaceRepository spaceRepository,
          [FromServices] IMapper automapper)
        {
            this.spaceRepository = spaceRepository;
            this.automapper = automapper;
        }
        public Task<Space> CreateAsync(Space space)
        {
            var result = spaceRepository.CreateAsync(automapper.Map<DbSpace>(space)).Result;
            return Task.FromResult(automapper.Map<Space>(result));
        }

        public Task<SpaceResponse> GetAsync(Guid spaceguid)
        {
            var result = spaceRepository.GetAsync(spaceguid).Result;


            return Task.FromResult(automapper.Map<SpaceResponse>(result));
        }

        public Task DeleteAsync(Guid spaceguid)
        {
            var source = spaceRepository.GetAsync(spaceguid).Result;
            if (source != null)
            {
                spaceRepository.DeleteAsync(source);
            }
            return Task.CompletedTask;
        }

        public Task<Space> UpdateAsync(Space target)
        {
            var source = spaceRepository.GetAsync(target.SpaceGuid).Result;
            if (source == null)
            {
                throw new NotImplementedException();
            }
            source.SpaceName = target.SpaceName;
            source.Description = target.Description;
            source.Capacity = target.Capacity;
            source.Floor = target.Floor;
            var result = spaceRepository.UpdateAsync(source).Result;
            return Task.FromResult(automapper.Map<Space>(result));
        }

        public Task<IEnumerable<SpaceResponse>> FindAsync(SpaceFilter filter)
        {
            var result = spaceRepository.FindAsync(filter).Result;

            return Task.FromResult(automapper.Map<IEnumerable<SpaceResponse>>(result));
        }


    }
}
