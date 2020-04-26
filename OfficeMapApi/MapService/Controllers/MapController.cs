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
        private readonly IMapService mapService;

        public MapController([FromServices] IMapService mapService)
        {
            this.mapService = mapService;
        }

        [HttpGet("mapFiles")]
        public async Task<ActionResult> GetMapFiles()
        {
            var result = await mapService.FindAsync();

            return Ok(result);
        }

        [HttpPost("mapFiles")]
        public async Task<ActionResult<MapFiles>> PostMapFiles([FromBody] MapFiles mapFile)
        {
            var result = await mapService.CreateAsync(mapFile);

            return Ok(result);
        }

        [HttpGet("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult<MapFiles>> GetMapFiles([FromRoute] Guid mapGuid)
        {
            var result = await mapService.GetAsync(mapGuid);
            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPut("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult<MapFiles>> PutMapFiles(
            [FromRoute] Guid mapGuid,
            [FromBody] MapFiles mapFile)

        {
            mapFile.MapGuid = mapGuid;
            var result = await mapService.UpdateAsync(mapFile);

            return Ok(result);
        }

        [HttpDelete("mapFiles/{maoFileGuid}")]
        public async Task<ActionResult> DeleteMapFiles([FromRoute] Guid mapGuid)
        {
            await mapService.DeleteAsync(mapGuid);

            return Ok();
        }
    }
}
