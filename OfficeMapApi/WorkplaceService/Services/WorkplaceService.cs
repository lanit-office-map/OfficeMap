using AutoMapper;
using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WorkplaceService.Clients;
using WorkplaceService.Database.Entities;
using WorkplaceService.Models;
using WorkplaceService.Models.RabbitMQ;
using WorkplaceService.Models.Services;
using WorkplaceService.Repository.Interfaces;

namespace WorkplaceService.Services
{
    public class WorkplaceService : IWorkplaceService
    {
        #region private fields
        private readonly IWorkplaceRepository workplaceRepository;
        private readonly IMapper automapper;
        private readonly ISpaceServiceClient spaceServiceClient;
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
          [FromServices] ISpaceServiceClient spaceServiceClient,
          [FromServices] IUserServiceClient userServiceClient)
        {
            this.workplaceRepository = workplaceRepository;
            this.automapper = automapper;
            this.spaceServiceClient = spaceServiceClient;
            this.userServiceClient = userServiceClient;
        }

        public async Task<Response<IEnumerable<WorkplaceResponse>>> FindAllAsync(WorkplaceRequest workplaceRequest)
        {
            //Get and validate space where the workplaces stand.
            var space = spaceServiceClient.GetSpaceIdAsync(
                workplaceRequest.OfficeGuid, workplaceRequest.SpaceGuid).Result;
            if (space == null)
            {
                return await Task.FromResult(Responses<IEnumerable<WorkplaceResponse>>.WorkplaceNotFounded);
            }

            var result = workplaceRepository.FindAllAsync(
                e => e.SpaceId == space.SpaceId && e.Obsolete == false);

            var response = new Response<IEnumerable<WorkplaceResponse>>(
                automapper.Map<IEnumerable<WorkplaceResponse>>(result));
            return await Task.FromResult(response);
        }

        public async Task<Response<WorkplaceResponse>> GetAsync(WorkplaceRequest workplaceRequest)
        {
            //Get and validate space where the workplaces stand.
            var space = spaceServiceClient.GetSpaceIdAsync(
                workplaceRequest.OfficeGuid, workplaceRequest.SpaceGuid).Result;
            if (space == null)
            {
                return await Task.FromResult(Responses<WorkplaceResponse>.SpaceNotFounded);
            }
            //Check that the specified WorkplaceGuid.
            if (workplaceRequest.WorkplaceGuid == null)
            {
                return await Task.FromResult(Responses<WorkplaceResponse>.WorkplaceNotFounded);
            }

            var result = workplaceRepository.GetAsync(workplaceRequest.WorkplaceGuid).Result;

            var response = new Response<WorkplaceResponse>(automapper.Map<WorkplaceResponse>(result));
            return await Task.FromResult(response);
        }

        public async Task<Response<Workplace>> CreateAsync(WorkplaceRequest workplaceRequest)
        {
            //Get and validate space where the workplaces will stand.
            var space = spaceServiceClient.GetSpaceIdAsync(
                workplaceRequest.OfficeGuid, workplaceRequest.SpaceGuid).Result;
            if (space == null)
            {
                return await Task.FromResult(Responses<Workplace>.SpaceNotFounded);
            }
            //Check that the specified employee exists.
            GetEmployeeRequest employeeRequest = new GetEmployeeRequest();
            var employeeGuid = workplaceRequest.Workplace.EmployeeGuid; //Not NullReferenceException if Workplase is not null
            employeeRequest.EmployeeGuid = employeeGuid;
            var employee = userServiceClient.GetUserIdAsync(employeeRequest).Result;
            if (employee == null)
            {
                return await Task.FromResult(Responses<Workplace>.EmployeeNotFounded);
            }
            //Check that the specified employee is not seated at another workplace.
            var workplace = workplaceRepository.FindAllAsync(
                e => e.EmployeeId == employee.EmployeeId && e.Obsolete == false);
            if (workplace != null)
            {
                return await Task.FromResult(Responses<Workplace>.EmployeeIsSittingAtAnotherWorkplace);
            }

            var result = workplaceRepository.CreateAsync(automapper.Map<DbWorkplace>(workplaceRequest.Workplace)).Result;

            var response = new Response<Workplace>(automapper.Map<Workplace>(result));
            return await Task.FromResult(response);
        }

        public async Task<Response<Workplace>> UpdateAsync(WorkplaceRequest workplaceRequest)
        {
            //Get and validate space where the workplaces will stand.
            var space = spaceServiceClient.GetSpaceIdAsync(
                workplaceRequest.OfficeGuid, workplaceRequest.SpaceGuid).Result;
            if (space == null)
            {
                return await Task.FromResult(Responses<Workplace>.SpaceNotFounded);
            }
            //Check that the specified employee exists.
            GetEmployeeRequest employeeRequest = new GetEmployeeRequest();
            var employeeGuid = workplaceRequest.Workplace.EmployeeGuid; //Not NullReferenceException if Workplase is not null
            employeeRequest.EmployeeGuid = employeeGuid;
            var employee = userServiceClient.GetUserIdAsync(employeeRequest).Result;
            if (employee == null)
            {
                return await Task.FromResult(Responses<Workplace>.EmployeeNotFounded);
            }
            //Get current workplace
            var workplace = workplaceRepository.GetAsync(workplaceRequest.WorkplaceGuid).Result;
            if (workplace == null)
            {
                return await Task.FromResult(Responses<Workplace>.WorkplaceNotFounded);
            }
            //If update employee
            if (workplace.EmployeeId != employee.EmployeeId)
            {
                //Check that the specified employee is not seated at another workplace.
                var hisWorkplace = workplaceRepository.FindAllAsync(
                    e => e.EmployeeId == employee.EmployeeId && e.Obsolete == false);
                if (hisWorkplace != null)
                {
                    return await Task.FromResult(Responses<Workplace>.EmployeeIsSittingAtAnotherWorkplace);
                }
            }

            workplace.EmployeeId = employee.EmployeeId;
            workplace.Map = automapper.Map<DbMapFile>(workplaceRequest.Workplace.WorkplaceMap);
            var result = workplaceRepository.UpdateAsync(workplace).Result;

            var response = new Response<Workplace>(automapper.Map<Workplace>(result));
            return await Task.FromResult(response);

            throw new NotImplementedException();
        }

        public async Task<Response<WorkplaceResponse>> DeleteAsync(WorkplaceRequest workplaceRequest)
        {
            //Get and validate space where the workplaces stand.
            var space = spaceServiceClient.GetSpaceIdAsync(
                workplaceRequest.OfficeGuid, workplaceRequest.SpaceGuid).Result;
            if (space == null)
            {
                return await Task.FromResult(Responses<WorkplaceResponse>.SpaceNotFounded);
            }
            //Get current workplace
            var workplace = workplaceRepository.GetAsync(workplaceRequest.WorkplaceGuid).Result;
            if (workplace == null)
            {
                return await Task.FromResult(Responses<WorkplaceResponse>.WorkplaceNotFounded);
            }
            _ = workplaceRepository.DeleteAsync(workplace);

            var response = new Response<WorkplaceResponse>(automapper.Map<WorkplaceResponse>(workplace));

            return await Task.FromResult(response);
        }
        #endregion
    }
}
