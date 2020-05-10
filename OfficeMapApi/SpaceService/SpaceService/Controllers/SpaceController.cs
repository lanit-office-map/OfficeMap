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
                
                foreach (var space in result)
                {
                    var _response = workplaceServiceClient.GetWorkplacesAsync(space.SpaceGuid).Result;
                    space.Workplaces = _response;
                    space.OfficeGuid = filter.OfficeGuid;
                    InnerSpaces(space.Spaces, officeGuid);
                }
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

        public async Task<ActionResult> GetSpace(
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
                var _response = workplaceServiceClient.GetWorkplacesAsync(spaceGuid).Result;
                result.Workplaces = _response;
                result.OfficeGuid = officeGuid;
                InnerSpaces(result.Spaces, officeGuid);
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

        public void InnerSpaces(ICollection<SpaceResponse> innerSpaces, Guid officeGuid)
        {
            foreach (var space in innerSpaces)
            {
                while (space.OfficeGuid != officeGuid)
                {
                    space.OfficeGuid = officeGuid;
                    InnerSpaces(space.Spaces, officeGuid);
                }
            }
            return;
        }

    }
}