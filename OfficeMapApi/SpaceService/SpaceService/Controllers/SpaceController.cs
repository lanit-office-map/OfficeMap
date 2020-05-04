using Microsoft.AspNetCore.Mvc;
using SpaceService.Services.Interfaces;
using SpaceService.Models;
using System;
using System.Threading.Tasks;
using SpaceService.Filters;
using SpaceService.Clients;

namespace SpaceService.Controllers
{
    [Route("SpaceService/[controller]/offices/{officeGuid}")]
    [ApiController]
    public class SpaceController : Controller
    {
        private readonly ISpacesService spaceService;
        private readonly OfficeServiceClient officeServiceClient;

        public SpaceController(
            [FromServices] ISpacesService spaceService,
            [FromServices] OfficeServiceClient officeServiceClient)
        {
            this.spaceService = spaceService;
            this.officeServiceClient = officeServiceClient;
        }

        [HttpGet("spaces")]

        public async Task<ActionResult> GetSpaces([FromRoute] Guid officeGuid)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                SpaceFilter filter = new SpaceFilter(response.OfficeId, response.OfficeGuid);
                var spaces = await spaceService.FindAsync(filter);
                foreach (var space in spaces)
                {
                    space.OfficeGuid = filter.OfficeGuid;
                }
                return Ok(spaces);
            }
        }

        [HttpPost("spaces")]

        public async Task<ActionResult<Space>> PostSpaces(
            [FromRoute] Guid officeGuid,
            [FromBody] Space space)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                space.OfficeId = response.OfficeId;
                var result = await spaceService.CreateAsync(space);
                return Ok(result);
            }
        }
        
        [HttpGet("spaces/{spaceGuid}")]

        public async Task<ActionResult<SpaceResponse>> GetSpace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                var result = await spaceService.GetAsync(spaceGuid);
                result.OfficeGuid = response.OfficeGuid;
                return Ok(result);
            }
        }

        [HttpPut("spaces/{spaceGuid}")]
        
        public async Task<ActionResult<Space>> PutSpace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid,
            [FromBody] Space target)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                target.OfficeId = response.OfficeId;
                target.SpaceGuid = spaceGuid;
                var result = await spaceService.UpdateAsync(target);
                return Ok(result);
            }
        }

        [HttpDelete("spaces/{spaceGuid}")]

        public async Task<ActionResult> DeleteSpace(
            [FromRoute] Guid officeGuid, 
            [FromRoute] Guid spaceGuid)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                var result = await spaceService.GetAsync(spaceGuid);
                await spaceService.DeleteAsync(spaceGuid);
                return Ok(result);
            }
        }
            
    }
}