using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

    }
}