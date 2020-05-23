using Microsoft.AspNetCore.Mvc;
using SpaceService.Services.Interfaces;
using SpaceService.Models;
using System;
using System.Threading.Tasks;
using SpaceService.Filters;
using SpaceService.Clients;
using SpaceService.Repository.Interfaces;
using System.Collections.Generic;

namespace SpaceService.Controllers
{
    [Route("SpaceService/[controller]/offices/{officeGuid}")]
    [ApiController]
    public class SpaceController : Controller
    {
        private readonly ISpacesService spaceService;
        private readonly OfficeServiceClient officeServiceClient;
        private readonly WorkplaceServiceClient workplaceServiceClient;

        public SpaceController(
            [FromServices] ISpacesService spaceService,
            [FromServices] OfficeServiceClient officeServiceClient,
            [FromServices] WorkplaceServiceClient workplaceServiceClient)
        {
            this.spaceService = spaceService;
            this.officeServiceClient = officeServiceClient;
            this.workplaceServiceClient = workplaceServiceClient;
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
                SpaceFilter filter = new SpaceFilter(response.OfficeId, response.Guid);
                var result = await spaceService.FindAsync(filter);
                Parallel.ForEach(result, async (_response) =>
                {
                    _response.Workplaces = await workplaceServiceClient.GetWorkplacesAsync(_response.SpaceId);
                });
                    return Ok(result);
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
            // TODO 
            // If browser sends a request, then we'll use officeServiceClient and workplaceServiceClient;
            // If another service sends a request (e.g. WorkplaceService needs a SpaceID)
            // then we'll use only officeServiceClient (since we don't need to get any workplaces);

            var response = await officeServiceClient.GetOfficeAsync(officeGuid);
            if (response == null)
            {
                return NotFound();
            }
            SpaceFilter filter = new SpaceFilter(response.OfficeId, response.Guid);  
            var result = await spaceService.GetAsync(spaceGuid, filter);
            result.Workplaces = await workplaceServiceClient.GetWorkplacesAsync(result.SpaceId);
            return Ok(result);
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