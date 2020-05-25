using AutoMapper;
using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using WorkplaceService.Clients.Interfaces;
using WorkplaceService.Database.Entities;
using WorkplaceService.Filters;
using WorkplaceService.Models;
using WorkplaceService.Repository.Interfaces;
using WorkplaceService.Services.Interfaces;

namespace WorkplaceService.Services
{
  public class WorkplaceService : IWorkplaceService
  {
    #region private fields
    private readonly IWorkplaceRepository workplaceRepository;
    private readonly IMapper automapper;
    private readonly IUserServiceClient userServiceClient;

    private static class Responses<T>
        where T : class
    {
      public static readonly Response<T> SpaceNotFounded =
          new Response<T>(HttpStatusCode.NotFound, $"Space by spaceGuid not founded");

      public static readonly Response<T> WorkplaceNotFounded =
          new Response<T>(HttpStatusCode.NotFound, $"Workplace by workplaceGuid not founded");

      public static readonly Response<T> EmployeeNotFounded =
          new Response<T>(HttpStatusCode.NotFound, $"Employee by employeeGuid not founded");

      public static Response<T> EmployeeIsSittingAtAnotherWorkplace =
          new Response<T>(HttpStatusCode.BadRequest, $"This employee is sitting at another workplace");
    }
    #endregion

    #region public methods
    public WorkplaceService(
      [FromServices] IWorkplaceRepository workplaceRepository,
      [FromServices] IMapper automapper,
      [FromServices] IUserServiceClient userServiceClient)
    {
      this.workplaceRepository = workplaceRepository;
      this.automapper = automapper;
      this.userServiceClient = userServiceClient;
    }

    public async Task<Response<IEnumerable<WorkplaceResponse>>> FindAllAsync(WorkplaceFilter filter)
    {
      var result = await workplaceRepository.FindAllAsync(filter);

      var response = new Response<IEnumerable<WorkplaceResponse>>(
          automapper.Map<IEnumerable<WorkplaceResponse>>(result));
      return response;
    }

    public async Task<Response<WorkplaceResponse>> GetAsync(Guid workplaceGuid)
    {
      var result = await workplaceRepository.GetAsync(workplaceGuid);

      var response = new Response<WorkplaceResponse>(automapper.Map<WorkplaceResponse>(result));
      return await Task.FromResult(response);
    }

    public async Task<Response<WorkplaceResponse>> CreateAsync(Workplace workplace)
    {
      var employeeResponse = await userServiceClient.GetUserAsync(
        new GetUserRequest()
        {
          UserGuid = workplace.UserGuid
        });
      if (employeeResponse.Status == ResponseResult.Error)
      {
        return new Response<WorkplaceResponse>(
          employeeResponse.Error.StatusCode, employeeResponse.Error.Message);
      }

      workplace.EmployeeId = employeeResponse.Result.Employee.EmployeeId;

      var result = await workplaceRepository.CreateAsync(automapper.Map<DbWorkplace>(workplace));

      var response = new Response<WorkplaceResponse>(automapper.Map<WorkplaceResponse>(result));
      return response;
    }

    public async Task<Response<WorkplaceResponse>> UpdateAsync(Workplace target)
    {
      //Get current workplace
      var workplace = await workplaceRepository.GetAsync(target.WorkspaceGuid);
      if (workplace == null)
      {
        return new Response<WorkplaceResponse>(
          HttpStatusCode.NotFound,
          $"Workplace with guid '{target.WorkspaceGuid}' was not found.");
      }

      var employeeResponse = await userServiceClient.GetUserAsync(
        new GetUserRequest()
        {
          UserGuid = target.UserGuid
        });
      if (employeeResponse.Status == ResponseResult.Error)
      {
        return new Response<WorkplaceResponse>(
          employeeResponse.Error.StatusCode, employeeResponse.Error.Message);
      }

      workplace.EmployeeId = employeeResponse.Result.Employee.EmployeeId;
      workplace.Map = automapper.Map<DbMapFile>(workplace.Map);
      var result = workplaceRepository.UpdateAsync(workplace).Result;

      var response = new Response<WorkplaceResponse>(automapper.Map<WorkplaceResponse>(result));
      return response;
    }

    public async Task DeleteAsync(Guid workplaceGuid)
    {
      //Get current workplace
      var workplace = await workplaceRepository.GetAsync(workplaceGuid);
      if (workplace != null)
      {
        await workplaceRepository.DeleteAsync(workplace);
      }
    }
    #endregion
  }
}
