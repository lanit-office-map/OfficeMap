using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Services.Interfaces;
using MapService.Models;

namespace MapService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService service;

        public MapController([FromServices] IMapService service)
        {
            this.service = service;
        }

        [HttpGet("mapFiles")]
        public async Task<ActionResult> GetMapFiles()
        {
            var result = await service.FindAsync();

            return Ok(result);
        }

        [HttpPost("mapFiles")]
        public async Task<ActionResult<MapFiles>> PostMapFiles([FromBody] MapFiles office)
        {
            var result = await service.CreateAsync(office);

            return Ok(result);
        }

        [HttpGet("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult<MapFiles>> GetMapFiles([FromRoute] Guid id)
        {
            var result = await service.GetAsync(id);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPut("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult<MapFiles>> PutMapFiles(
            [FromRoute] Guid officeGuid,
            [FromBody] MapFiles entity)

        {
            entity.MapGuid = officeGuid;
            var result = await service.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult> DeleteMapFiles([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);

            return Ok();
        }
    }
}
