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
        private readonly ISpaceTypeRepository spaceTypeRepository;

        public SpacesService(
          [FromServices] ISpaceRepository spaceRepository,
          [FromServices] IMapper automapper,
          [FromServices] ISpaceTypeRepository spaceTypeRepository)
        {
            this.spaceRepository = spaceRepository;
            this.automapper = automapper;
            this.spaceTypeRepository = spaceTypeRepository;
        }
        public Task<Space> CreateAsync(Space space)
        {

                var Space = automapper.Map<DbSpace>(space);
                Space.TypeId = spaceTypeRepository.GetAsync(space.SpaceTypeGuid).Result.TypeId;
                var result = spaceRepository.CreateAsync(Space).Result;
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
            source.TypeId = spaceTypeRepository.GetAsync(target.SpaceTypeGuid).Result.TypeId;
            source.SpaceName = target.SpaceName;
            source.Description = target.Description;
            source.Capacity = target.Capacity;
            source.Floor = target.Floor;
            var result = spaceRepository.UpdateAsync(source).Result;
            return Task.FromResult(automapper.Map<Space>(result));
        }

        public Task<IEnumerable<SpaceResponse>> FindAsync(SpaceFilter filter)
        {
            
            
            var spaces = spaceRepository.FindAsync(filter).Result;
            var result = automapper.Map<IEnumerable<SpaceResponse>>(spaces);
            foreach (DbSpace space in spaces)
            {
                SpaceResponse temp = automapper.Map<SpaceResponse>(space);
                if (space.InverseParent != null)
                {
                    foreach (SpaceResponse spaceResponse in result)
                    {
                        if (spaceResponse.SpaceGuid == space.SpaceGuid)
                        {
                            spaceResponse.Spaces = temp.Spaces;
                            spaceResponse.SpaceType = temp.SpaceType;
                            spaceResponse.Map = temp.Map;                           
                        }
                    }
                }
            }

            return Task.FromResult(result);
        }
         

    }
}
