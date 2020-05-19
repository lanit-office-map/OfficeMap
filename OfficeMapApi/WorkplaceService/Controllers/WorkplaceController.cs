using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WorkplaceService.Models;
using WorkplaceService.Models.Services;
using WorkplaceService.Services;

namespace WorkplaceService.Controllers
{
    [Route("[controller]")]
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
            this.workplaceService = workplaceService;        }

       
		[HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult> GetWorkplaces(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid)
        {
            var spaceId = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (spaceId == 0)
            {
                return BadRequest();
            }
			
			var filter = new WorkplaceFilter(spaceId);
            var response = await workplaceService.FindAllAsync(filter);
			
            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

            

            return Ok(result);
        }
		
		[HttpPost("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult> PostWorkplace(
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
			
            var response = await workplaceService.CreateAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, Workplace = workplace });

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpGet("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult> GetWorkplace(
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
            var response = await workplaceService.GetAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid });

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpPut("offices/{officeGuid}/spaces/{spaceGuid}/workplaces/{workplaceGuid}")]
        public async Task<ActionResult> PutWorkplace(
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
            var response = await workplaceService.CreateAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid, Workplace = workplace });

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
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
            var response = await workplaceService.DeleteAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid, WorkplaceGuid = workplaceGuid });

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }
        #endregion
    }
}
