using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Database.Entities;
using MapService.Models;
using MapService.Repository.Interfaces;
using AutoMapper;
using MapService.Repository.Filters;
using MapService.Services.Interfaces;

namespace MapService.Services
{
    public class MapService : IMapService
    {
        private readonly IMapFilesRepository mapRepository;
        private readonly IMapper automapper;

        public MapService(
          [FromServices] IMapFilesRepository mapRepository,
          [FromServices] IMapper automapper)
        {
            this.mapRepository = mapRepository;
            this.automapper = automapper;
        }

        public Task<MapFiles> CreateAsync(MapFiles mapFile)
        {
            var result = mapRepository.CreateAsync(automapper.Map<DbMapFiles>(mapFile)).Result;

            return Task.FromResult(automapper.Map<MapFiles>(result));
        }

        public Task DeleteAsync(Guid mapGuid)
        {
            var source = mapRepository.GetAsync(mapGuid).Result;
            if (source != null)
            {
                mapRepository.DeleteAsync(source);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<MapFiles>> FindAsync(MapFilesFilter filter = null)
        {
            var result = mapRepository.FindAsync(filter).Result;

            return Task.FromResult(automapper.Map<IEnumerable<MapFiles>>(result));
        }

        public Task<MapFiles> GetAsync(Guid mapGuid)
        {
            var result = mapRepository.GetAsync(mapGuid).Result;

            return Task.FromResult(automapper.Map<MapFiles>(result));
        }

        public Task<MapFiles> UpdateAsync(MapFiles mapFile)
        {
            throw new NotImplementedException(); //TODO
        }
    }
}
