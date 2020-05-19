using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkplaceService.Models;
using WorkplaceService.Models.Services;
using WorkplaceService.Services;

namespace WorkplaceService.Controllers
{
    [Route("WorkplaceService/[controller]")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        #region private fields
        private readonly IWorkplaceService workplaceService;
        #endregion

        #region public methods
        public WorkplaceController(
        [FromServices] IWorkplaceService workplaceService)
        {
            this.workplaceService = workplaceService;
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<Response<IEnumerable<WorkplaceResponse>>> GetWorkplaces(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid)
        {
            var response = await workplaceService.FindAllAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid });

            return response;
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<Response<WorkplaceResponse>> GetWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var response = await workplaceService.GetAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid });

            return response;
        }

        [HttpPost("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<Response<Workplace>> PostWorkplace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid, 
            [FromBody] Workplace workplace)
        {
            var response = await workplaceService.CreateAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, Workplace = workplace });

            return response;
        }

        [HttpPut("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<Response<Workplace>> PutWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid,
            [FromBody] Workplace workplace)

        {
            var response = await workplaceService.CreateAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid, Workplace = workplace });

            return response;
        }

        [HttpDelete("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<Response<WorkplaceResponse>> DeleteWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var response = await workplaceService.DeleteAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid });

            return response;
        }
        #endregion
    }
}
