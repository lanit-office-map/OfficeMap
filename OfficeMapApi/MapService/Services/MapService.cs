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
        private readonly IMapFilesRepository repository;
        private readonly IMapper automapper;

        public MapService(
          [FromServices] IMapFilesRepository repository,
          [FromServices] IMapper automapper)
        {
            this.repository = repository;
            this.automapper = automapper;
        }

        public Task<MapFiles> CreateAsync(MapFiles entity)
        {
            var result = repository.CreateAsync(automapper.Map<DbMapFiles>(entity)).Result;

            return Task.FromResult(automapper.Map<MapFiles>(result));
        }

        public Task DeleteAsync(Guid id)
        {
            var source = repository.GetAsync(id).Result;
            if (source != null)
            {
                repository.DeleteAsync(source);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<MapFiles>> FindAsync(MapFilesFilter filter = null)
        {
            var result = repository.FindAsync(filter).Result;

            return Task.FromResult(automapper.Map<IEnumerable<MapFiles>>(result));
        }

        public Task<MapFiles> GetAsync(Guid id)
        {
            var result = repository.GetAsync(id).Result;

            return Task.FromResult(automapper.Map<MapFiles>(result));
        }

        public Task<MapFiles> UpdateAsync(MapFiles entity)
        {
            throw new NotImplementedException(); //TODO
        }
    }
}
