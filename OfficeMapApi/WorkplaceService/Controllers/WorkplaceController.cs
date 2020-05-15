using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkplaceService.Clients;
using WorkplaceService.Filters;
using WorkplaceService.Models;
using WorkplaceService.Services;

namespace WorkplaceService.Controllers
{
    [Route("WorkplaceService/[controller]")]
    [ApiController]
    public class WorkplaceController : ControllerBase
    {
        #region private fields
        private readonly IWorkplaceService workplaceService;
        private readonly ISpaceServiceClient spaceServiceClient;
        private readonly IUserServiceClient userServiceClient;
        #endregion

        #region public methods
        public WorkplaceController(
        [FromServices] IWorkplaceService workplaceService,
        [FromServices] ISpaceServiceClient spaceServiceClient,
        [FromServices] IUserServiceClient userServiceClient)
        {
            this.workplaceService = workplaceService;
            this.spaceServiceClient = spaceServiceClient;
            this.userServiceClient = userServiceClient;
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult<IEnumerable<WorkplaceResponse>>> GetWorkplaces(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid)
        {
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }

            var filter = new WorkplaceFilter(spaceId);
            var result = await workplaceService.FindAllAsync(filter);

            return Ok(result);
        }

        [HttpPost("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult<WorkplaceResponse>> PostWorkplace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid, 
            [FromBody] Workplace workplace)
        {
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }

            var employee = userServiceClient.GetUserIdAsync(workplace.EmployeeGuid).Result;
            if (employee == null)
            {
                return BadRequest();
            }

            workplace.SpaceId = spaceId;
            workplace.EmployeeId = employee.EmployeeId;

            var result = await workplaceService.CreateAsync(workplace);
            return Ok(result);
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult<WorkplaceResponse>> GetWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }
            // Implement filter on the Guid level
            WorkplaceFilter filter = new WorkplaceFilter(spaceId);
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
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }

            var currentWorkplace = await workplaceService.GetAsync(workplaceGuid);
            if (currentWorkplace == null)
            {
                return NotFound();
            }

            var employee = userServiceClient.GetUserIdAsync(workplace.EmployeeGuid).Result;
            if (employee == null)
            {
                return BadRequest();
            }

            workplace.SpaceId = spaceId;
            workplace.Guid = currentWorkplace.Guid;
            workplace.EmployeeId = employee.EmployeeId;

            var result = await workplaceService.UpdateAsync(workplace);
            return Ok(result);
        }

        [HttpDelete("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult> DeleteWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromRoute] Guid workplaceGuid)
        {
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }

            await workplaceService.DeleteAsync(workplaceGuid);
            return Ok();
        }
        #endregion
    }
}
