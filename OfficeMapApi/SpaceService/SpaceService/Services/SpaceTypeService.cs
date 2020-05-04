﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using AutoMapper;
using SpaceService.Filters;
using SpaceService.Database.Entities;
using System;
using SpaceService.Services.Interfaces;

namespace SpaceService.Services
{
    public class SpaceTypeService : ISpaceTypeService
    {
        private readonly ISpaceTypeRepository spacetypeRepository;
        private readonly IMapper automapper;


        public SpaceTypeService(
          [FromServices] ISpaceTypeRepository spacetypeRepository,
          [FromServices] IMapper automapper)
        {
            this.spacetypeRepository = spacetypeRepository;
            this.automapper = automapper;
        }

        public Task<SpaceType> CreateAsync(SpaceType spacetype)
        {
            var result = spacetypeRepository.CreateAsync(automapper.Map<DbSpaceType>(spacetype)).Result;
            return Task.FromResult(automapper.Map<SpaceType>(result));
        }

        public Task<SpaceTypeResponse> GetAsync(Guid spacetypeguid)
        {
            var result = spacetypeRepository.GetAsync(spacetypeguid).Result;

            return Task.FromResult(automapper.Map<SpaceTypeResponse>(result));
        }

        public Task DeleteAsync(Guid spacetypeguid)
        {
            var source = spacetypeRepository.GetAsync(spacetypeguid).Result;
            if (source != null)
            {
                spacetypeRepository.DeleteAsync(source);
            }
            return Task.CompletedTask;
        }

        public Task<SpaceType> UpdateAsync(SpaceType target)
        {
            var source = spacetypeRepository.GetAsync(target.SpaceTypeGuid).Result;
            if (source == null)
            {
                throw new NotImplementedException();
            }
            source.Bookable = target.Bookable;
            source.Name = target.Name;
            source.Description = target.Description;
            var result = spacetypeRepository.UpdateAsync(source).Result;
            return Task.FromResult(automapper.Map<SpaceType>(result));
        }

        public Task<IEnumerable<SpaceTypeResponse>> FindAsync(SpaceTypeFilter filter)
        {
            var result = spacetypeRepository.FindAsync(filter).Result;

            return Task.FromResult(automapper.Map<IEnumerable<SpaceTypeResponse>>(result));
        }



    }
}
