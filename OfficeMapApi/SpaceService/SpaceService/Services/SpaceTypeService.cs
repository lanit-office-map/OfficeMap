using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using AutoMapper;
using SpaceService.Filters;
using SpaceService.Database.Entities;
using System;
using System.Net;
using Common.Response;
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

    public async Task<Response<SpaceTypeResponse>> CreateAsync(SpaceType spacetype)
    {
      var result = await spacetypeRepository.CreateAsync(automapper.Map<DbSpaceType>(spacetype));
      return new Response<SpaceTypeResponse>(
        automapper.Map<SpaceTypeResponse>(result));
    }

    public async Task<Response<SpaceTypeResponse>> GetAsync(Guid spacetypeguid)
    {
      var result = await spacetypeRepository.GetAsync(spacetypeguid);

      return new Response<SpaceTypeResponse>(automapper.Map<SpaceTypeResponse>(result));
    }

    public async Task DeleteAsync(Guid spacetypeguid)
    {
      var source = await spacetypeRepository.GetAsync(spacetypeguid);
      if (source != null)
      {
        await spacetypeRepository.DeleteAsync(source);
      }
    }

    public async Task<Response<SpaceTypeResponse>> UpdateAsync(SpaceType target)
    {
      var source = await spacetypeRepository.GetAsync(target.SpaceTypeGuid);
      if (source == null)
      {
        return new Response<SpaceTypeResponse>(HttpStatusCode.NotFound,
          $"SpaceType with guid '{target.SpaceTypeGuid} was not found.'");
      }
      source.Bookable = target.Bookable;
      source.Name = target.Name;
      source.Description = target.Description;
      var result = await spacetypeRepository.UpdateAsync(source);
      return new Response<SpaceTypeResponse>(automapper.Map<SpaceTypeResponse>(result));
    }

    public async Task<Response<IEnumerable<SpaceTypeResponse>>> FindAllAsync()
    {
      var result = await spacetypeRepository.FindAllAsync();

      return new Response<IEnumerable<SpaceTypeResponse>>(automapper.Map<IEnumerable<SpaceTypeResponse>>(result));
    }
  }
}
