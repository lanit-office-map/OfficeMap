using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkplaceService.Clients;
using WorkplaceService.Models;
using WorkplaceService.Services;

namespace WorkplaceService.Controllers
{
    [Route("WorkplaceService/[controller]")]
    [ApiController]
    public class WorkplaceController
    {
        #region private fields
        private readonly IWorkplaceService workplaceService;
        private readonly ISpaceServiceClient spaceServiceClient;
        #endregion

        #region public methods
        public WorkplaceController(
        [FromServices] IWorkplaceService workplaceService,
        [FromServices] ISpaceServiceClient spaceServiceClient)
        {
            this.workplaceService = workplaceService;
            this.spaceServiceClient = spaceServiceClient;
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult<IEnumerable<WorkplaceResponse>>> GetWorkplaces(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid)
        {
            var space = spaceServiceClient.GetSpaceGuidsAsync(officeGuid, spaceGuid).Result;

            var result = await workplaceService.(space.SpaceId);

            return Ok(result);
        }

        [HttpPost("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult<WorkplaceResponse>> PostWorkplace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid, 
            [FromBody] Workplace workplace)
        {
            var result = await workplaceService.CreateAsync(officeGuid, spaceGuid, workplace);
            return Ok(result);
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult<WorkplaceResponse>> GetWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var result = await workplaceService.GetAsync(workplaceGuid);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPut("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult<WorkplaceResponse>> PutWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid,
            [FromBody] Workplace workplace)

        {
            var result = await workplaceService.UpdateAsync(officeGuid, spaceGuid, workplaceGuid, workplace);
            return Ok(result);
        }

        [HttpDelete("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult> DeleteWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            await workplaceService.DeleteAsync(officeGuid, spaceGuid, workplaceGuid);
            return Ok();
        }
        #endregion
    }
}
