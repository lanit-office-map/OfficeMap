using System.Threading.Tasks;
using System;
using Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Services.Interfaces;

namespace SpaceService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class SpaceTypeController : Controller
  {
    private readonly ISpaceTypeService spacetypeService;

    public SpaceTypeController(
    [FromServices] ISpaceTypeService spacetypeService)
    {
      this.spacetypeService = spacetypeService;
    }
    [HttpGet("spacetypes")]
    public async Task<ActionResult> GetSpaceTypes()
    {
      var response = await spacetypeService.FindAllAsync();

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpPost("spacetypes")]
    public async Task<ActionResult<SpaceType>> PostSpaceTypes([FromBody] SpaceType spacetype)
    {
      var response = await spacetypeService.CreateAsync(spacetype);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }



    [HttpGet("spacetypes/{spacetypeGuid}")]
    public async Task<ActionResult<SpaceTypeResponse>> GetSpaceType([FromRoute] Guid spacetypeGuid)
    {
      var response = await spacetypeService.GetAsync(spacetypeGuid);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }



    [HttpPut("spacetypes/{spacetypeGuid}")]
    public async Task<ActionResult<SpaceType>> PutSpaceType(
        [FromRoute] Guid spacetypeGuid,
        [FromBody] SpaceType target)

    {
      target.SpaceTypeGuid = spacetypeGuid;
      var response = await spacetypeService.UpdateAsync(target);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    [HttpDelete("spacetypes/{spacetypeGuid}")]
    public async Task<ActionResult> DeleteSpaceType([FromRoute] Guid spacetypeGuid)
    {
      await spacetypeService.DeleteAsync(spacetypeGuid);
      return NoContent(); ;
    }
  }
}