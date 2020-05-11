using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        #endregion

        #region public methods
        public WorkplaceController(
        [FromServices] IWorkplaceService workplaceService)
        {
            this.workplaceService = workplaceService;
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult> GetWorkplacesBySpaceGuid(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid)
        {
            var result = await workplaceService.GetAllBySpaceGuid(officeGuid, spaceGuid);

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
        public async Task<ActionResult<WorkplaceResponse>> GetWorkplaceByGuid(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var result = await workplaceService.GetAsync(officeGuid, spaceGuid, workplaceGuid);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPut("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult<WorkplaceResponse>> PutWorkplaceByGuid(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid,
            [FromBody] Workplace workplace)

        {
            var result = await workplaceService.UpdateAsync(officeGuid, spaceGuid, workplaceGuid, workplace);
            return Ok(result);
        }

        [HttpDelete("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult> DeleteWorkplaceByGuid(
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
