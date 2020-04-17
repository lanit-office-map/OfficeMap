using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using SpaceService.Services.Interface;
using AutoMapper;
using SpaceService.Repository.Filters;

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

        public Task<IEnumerable<SpaceType>> FindAsync(SpaceTypeFilter filter)
        {
            var result = spacetypeRepository.FindAsync(filter).Result;

            return Task.FromResult(automapper.Map<IEnumerable<SpaceType>>(result));
        }

    }
}
