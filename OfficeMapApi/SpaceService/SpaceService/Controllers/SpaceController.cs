using Microsoft.AspNetCore.Mvc;
using SpaceService.Services.Interfaces;
using SpaceService.Models;
using System;
using System.Threading.Tasks;
using SpaceService.Filters;
using SpaceService.RabbitMQ;
using System.Collections.Generic;
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

        public async Task<ActionResult<Space>> GetSpaces([FromRoute] Guid officeGuid)
        {
            Console.WriteLine("SpaceService awaiting Office model");
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            SpaceFilter filter = new SpaceFilter(response.OfficeId);
            var result = await spaceService.FindAsync(filter);
            return Ok(result);
        }

        [HttpPost("spaces")]

        public async Task<ActionResult<Space>> PostSpaces(
            [FromRoute] Guid officeGuid,
            [FromBody] Space space)
        {
            var response = officeServiceClient.GetOfficeAsync(officeGuid).Result;
            officeServiceClient.Dispose();
            throw new NotImplementedException();

        }
    }
}