using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Services.Interface;

namespace SpaceService.Controllers
{
    [Route("SpaceTypeService/[controller]")]
    [ApiController]
    public class SpaceTypeController : ControllerBase
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
            var result = await spacetypeService.FindAsync();

            return Ok(result);
        }

        [HttpPost("spacetypes")]
        public async Task<ActionResult<SpaceType>> PostSpaceTypes([FromBody] SpaceType spacetype)
        {
            var result = await spacetypeService.CreateAsync(spacetype);
            return Ok(result);
        }



        [HttpGet("spacetypes/{spacetypeGuid}")]
        public async Task<ActionResult<SpaceType>> GetSpaceType([FromRoute] Guid spacetypeGuid)
        {
            var result = await spacetypeService.GetAsync(spacetypeGuid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        [HttpPut("spacetypes/{spacetypeGuid}")]
        public async Task<ActionResult<SpaceType>> PutSpaceType(
            [FromRoute] Guid spacetypeGuid,
            [FromBody] SpaceType target)

        {
            target.SpaceTypeGuid = spacetypeGuid;
            var result = await spacetypeService.UpdateAsync(target);
            return Ok(result);
        }

        [HttpDelete("spacetypes/{spacetypeGuid}")]
        public async Task<ActionResult> DeleteSpaceType([FromRoute] Guid spacetypeGuid)
        {
            await spacetypeService.DeleteAsync(spacetypeGuid);
            return Ok();
        }
    }
}