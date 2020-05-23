using Common.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using WorkplaceService.Clients.Interfaces;
using WorkplaceService.Filters;
using WorkplaceService.Models;
using WorkplaceService.Services.Interfaces;

namespace WorkplaceService.Controllers
{
  [Route("[controller]/offices/{officeGuid}/spaces/{spaceGuid}")]
  [ApiController]
  public class WorkplaceController : Controller
  {
    #region private fields
    private readonly IWorkplaceService workplaceService;
    private readonly ISpaceServiceClient spaceServiceClient;
    private readonly IOfficeServiceClient officeServiceClient;
    private Response<GetOfficeResponse> officeResponse;
    private Response<GetSpaceResponse> spaceResponse;
    #endregion

    #region public methods
    public WorkplaceController(
        [FromServices] IWorkplaceService workplaceService,
        [FromServices] ISpaceServiceClient spaceServiceClient,
        [FromServices] IOfficeServiceClient officeServiceClient)
    {
      this.workplaceService = workplaceService;
      this.spaceServiceClient = spaceServiceClient;
      this.officeServiceClient = officeServiceClient;
    }

    public override async Task OnActionExecutionAsync(
      ActionExecutingContext context,
      ActionExecutionDelegate next)
    {
      var officeGuid = (Guid)context.ActionArguments["officeGuid"];
      officeResponse = await officeServiceClient.GetOfficeAsync(new GetOfficeRequest
      {
        OfficeGuid = officeGuid
      });
      if (officeResponse.Status == ResponseResult.Error)
      {
        context.Result =
          StatusCode((int)officeResponse.Error.StatusCode, officeResponse.Error.Message);
      }

      var spaceGuid = (Guid)context.ActionArguments["spaceGuid"];
      spaceResponse = await spaceServiceClient.GetSpaceAsync(
        new GetSpaceRequest
        {
          SpaceGuid = spaceGuid
        });
      if (spaceResponse.Status == ResponseResult.Error)
      {
        context.Result =
          StatusCode((int)spaceResponse.Error.StatusCode, spaceResponse.Error.Message);
      }

      await base.OnActionExecutionAsync(context, next);
    }

    [HttpGet("workplaces")]
    public async Task<ActionResult> GetWorkplaces(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid)
    {
      var response = await workplaceService.FindAllAsync(new WorkplaceFilter(spaceResponse.Result.SpaceId));

      return response.Status == ResponseResult.Success
      ? Ok(response.Result)
      : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpPost("workplaces")]
    public async Task<ActionResult> PostWorkplace(
            [FromRoute] Guid officeGuid,
            [FromRoute] Guid spaceGuid,
            [FromBody] Workplace workplace)
    {
      var response = await workplaceService.CreateAsync(workplace);

      return response.Status == ResponseResult.Success
      ? Ok(response.Result)
      : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpGet("workplaces/{workplaceGuid}")]
    public async Task<ActionResult> GetWorkplace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid,
        [FromRoute] Guid workplaceGuid)
    {
      var response = await workplaceService.GetAsync(workplaceGuid);

      return response.Status == ResponseResult.Success
      ? Ok(response.Result)
      : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }


    [HttpPut("workplaces/{workplaceGuid}")]
    public async Task<ActionResult> PutWorkplace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid,
        [FromRoute] Guid workplaceGuid,
        [FromBody] Workplace workplace)
    {
        workplace.SpaceId = spaceResponse.Result.SpaceId;
        workplace.WorkspaceGuid = workplaceGuid;
        var response = await workplaceService.UpdateAsync(workplace);

        return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }


    [HttpDelete("workplaces/{workplaceGuid}")]
    public async Task<ActionResult> DeleteWorkplace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid,
        [FromRoute] Guid workplaceGuid)
    {
     await workplaceService.DeleteAsync(workplaceGuid);

      return NoContent();
    }
    #endregion
  }
}
