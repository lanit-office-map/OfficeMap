using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Database.Entities;
using SpaceService.Filters;
using SpaceService.Models;
using SpaceService.Repository.Interfaces;
using SpaceService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;
using SpaceService.Clients.Interfaces;

namespace SpaceService.Services
{
  public class SpacesService : ISpacesService
  {
    private readonly ISpaceRepository spaceRepository;
    private readonly IMapper automapper;
    private readonly ISpaceTypeRepository spaceTypeRepository;
    private readonly IWorkplaceServiceClient workplaceServiceClient;

    public SpacesService(
      [FromServices] ISpaceRepository spaceRepository,
      [FromServices] IMapper automapper,
      [FromServices] ISpaceTypeRepository spaceTypeRepository,
      [FromServices] IWorkplaceServiceClient workplaceServiceClient)
    {
      this.spaceRepository = spaceRepository;
      this.automapper = automapper;
      this.spaceTypeRepository = spaceTypeRepository;
      this.workplaceServiceClient = workplaceServiceClient;
    }
    public async Task<Response<SpaceResponse>> CreateAsync(Space space)
    {
      var spaceType = await spaceTypeRepository.GetAsync(space.SpaceTypeGuid);
      if (spaceType == null)
      {
        return new Response<SpaceResponse>(
          HttpStatusCode.NotFound,
          $"SpaceType with guid '{space.SpaceTypeGuid}' was not found.");
      }
      var dbSpace = automapper.Map<DbSpace>(space);
      dbSpace.MapFile = automapper.Map<DbMapFile>(space.Map);
      dbSpace.TypeId = spaceType.TypeId;
      var result = await spaceRepository.CreateAsync(dbSpace);
      return new Response<SpaceResponse>(automapper.Map<SpaceResponse>(result));
    }

    public async Task<Response<SpaceResponse>> GetAsync(Guid spaceguid)
    {
      var space = await spaceRepository.GetAsync(spaceguid);
      var result = automapper.Map<SpaceResponse>(space);
      var response = await workplaceServiceClient.GetWorkplacesAsync(
        new GetWorkplacesRequest
        {
          SpaceId = space.SpaceId
        });

      if (response.Status == ResponseResult.Success)
      {
        result.Workplaces = response.Result;
      }

      return new Response<SpaceResponse>(result);
    }

    public async Task DeleteAsync(Guid spaceguid)
    {
      var source = await spaceRepository.GetAsync(spaceguid);
      if (source != null)
      {
        await spaceRepository.DeleteAsync(source);
      }
    }

    public async Task<Response<SpaceResponse>> UpdateAsync(Space target)
    {
      var source = await spaceRepository.GetAsync(target.SpaceGuid);
      if (source == null)
      {
        return new Response<SpaceResponse>(
          HttpStatusCode.NotFound,
          $"Space with guid '{target.SpaceGuid}' was not found.");
      }

      var spaceType = await spaceTypeRepository.GetAsync(target.SpaceTypeGuid);
      if (spaceType == null)
      {
        return new Response<SpaceResponse>(
          HttpStatusCode.NotFound,
          $"SpaceType with guid '{target.SpaceTypeGuid}' was not found.");
      }
      source.TypeId = spaceType.TypeId;
      source.SpaceName = target.SpaceName;
      source.Description = target.Description;
      source.Capacity = target.Capacity;
      source.Floor = target.Floor;
      source.MapFile = automapper.Map<DbMapFile>(target.Map);
      var result = await spaceRepository.UpdateAsync(source);
      return new Response<SpaceResponse>(automapper.Map<SpaceResponse>(result));
    }

    public async Task<Response<IEnumerable<SpaceResponse>>> FindAllAsync(SpaceFilter filter)
    {
      var spaces = await spaceRepository.FindAllAsync(filter);
      var result = automapper.Map<IEnumerable<SpaceResponse>>(spaces);
      var spaceResponses = result.ToList();
      Parallel.ForEach(spaceResponses, async (space) =>
      {
        var response = await workplaceServiceClient.GetWorkplacesAsync(
          new GetWorkplacesRequest
          {
            SpaceId = space.SpaceId
          });

        if (response.Status == ResponseResult.Success)
        {
          space.Workplaces = response.Result;
        }
      });
      return new Response<IEnumerable<SpaceResponse>>(spaceResponses);
    }
  }
}
