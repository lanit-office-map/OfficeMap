using Microsoft.AspNetCore.Mvc;
using SpaceService.Services.Interfaces;
using SpaceService.Models;
using System;
using System.Threading.Tasks;
using SpaceService.Filters;
using SpaceService.Clients;
using SpaceService.Repository.Interfaces;
using System.Collections.Generic;
using Common.RabbitMQ.Models;
using Common.Response;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS;
using SpaceService.Clients.Interfaces;

namespace SpaceService.Controllers
{
  [Route("[controller]/offices/{officeGuid}")]
  [ApiController]
  public class SpaceController : Controller
  {
    private readonly ISpacesService spaceService;
    private readonly IOfficeServiceClient officeServiceClient;
    private readonly IWorkplaceServiceClient workplaceServiceClient;
    private Response<GetOfficeResponse> officeResponse;

    public SpaceController(
        [FromServices] ISpacesService spaceService,
        [FromServices] IOfficeServiceClient officeServiceClient,
        [FromServices] IWorkplaceServiceClient workplaceServiceClient)
    {
      this.spaceService = spaceService;
      this.officeServiceClient = officeServiceClient;
      this.workplaceServiceClient = workplaceServiceClient;
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

      await base.OnActionExecutionAsync(context, next);
    }

    [HttpGet("spaces")]
    public async Task<ActionResult<IEnumerable<SpaceResponse>>> GetSpaces([FromRoute] Guid officeGuid)
    {
      var response = await spaceService.FindAllAsync(
        new SpaceFilter(officeResponse.Result.OfficeId));

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpPost("spaces")]
    public async Task<ActionResult<SpaceResponse>> PostSpaces(
        [FromRoute] Guid officeGuid,
        [FromBody] Space space)
    {
      space.OfficeId = officeResponse.Result.OfficeId;
      var response = await spaceService.CreateAsync(space);
      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpGet("spaces/{spaceGuid}")]
    public async Task<ActionResult<SpaceResponse>> GetSpace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid)
    {
      // TODO
      // If browser sends a request, then we'll use officeServiceClient and workplaceServiceClient;
      // If another service sends a request (e.g. WorkplaceService needs a SpaceID)
      // then we'll use only officeServiceClient (since we don't need to get any workplaces);

      var response = await spaceService.GetAsync(spaceGuid);
      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpPut("spaces/{spaceGuid}")]
    public async Task<ActionResult<SpaceResponse>> PutSpace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid,
        [FromBody] Space target)
    {
      target.OfficeId = officeResponse.Result.OfficeId;
      target.SpaceGuid = spaceGuid;
      var response = await spaceService.UpdateAsync(target);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpDelete("spaces/{spaceGuid}")]

    public async Task<ActionResult> DeleteSpace(
        [FromRoute] Guid officeGuid,
        [FromRoute] Guid spaceGuid)
    {
      await spaceService.DeleteAsync(spaceGuid);

      return NoContent();
    }
  }
}