using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Models;
using OfficeService.Services.Interface;
using System;

namespace OfficeService.Controllers
{
    [Route("OfficeService/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        #region private fields
        private readonly IOfficeService officeService;
        #endregion

        #region public methods
        public OfficeController(
        [FromServices] IOfficeService officeService)
        {
            this.officeService = officeService;
        }

        [HttpGet("offices")]
        public async Task<ActionResult> GetOffices()
        {
            var result = await officeService.FindAllAsync();

            return Ok(result);
        }

        [HttpPost("offices")]
        public async Task<ActionResult<Office>> PostOffices([FromBody] Office office)
        {
            var result = await officeService.CreateAsync(office);
            return Ok(result);
        }

        [HttpGet("offices/{officeGuid}")]
        public async Task<ActionResult<Office>> GetOffice([FromRoute] Guid officeGuid)
        {
            var result = await officeService.GetAsync(officeGuid);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("offices/{officeGuid}")]
        public async Task<ActionResult<Office>> PutOffice(
            [FromRoute] Guid officeGuid,
            [FromBody] Office target)

        {
            target.Guid = officeGuid;
            var result = await officeService.UpdateAsync(target);
            return Ok(result);
        }

        [HttpDelete("offices/{officeGuid}")]
        public async Task<ActionResult> DeleteOffice([FromRoute] Guid officeGuid)
        {
            await officeService.DeleteAsync(officeGuid);
            return Ok();
        }
        #endregion
    }
}