using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WorkplaceService.Clients;
using WorkplaceService.Filters;
using WorkplaceService.Models;
using WorkplaceService.Models.Services;
using WorkplaceService.Services;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Controllers
{
    [Route("[controller]")]
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
        public async Task<ActionResult> GetWorkplaces(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid)
        {
            var space = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (space == null)
            {
                return BadRequest();
            }

            var response = await workplaceService.FindAllAsync(
                new WorkplaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid });


            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }
		
		[HttpPost("offices/{officeGuid}/spaces/{spaceGuid}/workplaces")]
        public async Task<ActionResult> PostWorkplace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid, 
            [FromBody] Workplace workplace)
        {
			var space = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (space == null)
            {
                return BadRequest();
            }
            Employee employeeRequest = new Employee()
            {
                EmployeeGuid = workplace.EmployeeGuid
            };			
			 var employee = userServiceClient.GetUserIdAsync(employeeRequest).Result;
            if (employee == null)
            {
                return BadRequest();
            }
            workplace.SpaceGuid = spaceGuid;
            workplace.EmployeeGuid = employee.EmployeeGuid;
            

			
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
            var space = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (space == null)
            {
                return BadRequest();
            }
            // Implement filter on the Guid level
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
            var space = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (space == null)
            {
                return BadRequest();
            }

            /*    var currentWorkplace = await workplaceService.GetAsync(workplaceGuid);
                if (currentWorkplace == null)
                {
                    return NotFound();
                }
                */

            Employee employeeRequest = new Employee()
            {
                EmployeeGuid = workplace.EmployeeGuid
            };
            var employee = userServiceClient.GetUserIdAsync(employeeRequest).Result;
            if (employee == null)
            {
                return BadRequest();
            }

            workplace.SpaceGuid = spaceGuid;
         //   workplace.WorkplaceGuid = currentWorkplace.Guid;
            workplace.EmployeeGuid = employee.EmployeeGuid;
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
            var space = spaceServiceClient.GetSpaceIdAsync(officeGuid, spaceGuid).Result;
            if (space == null)
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
