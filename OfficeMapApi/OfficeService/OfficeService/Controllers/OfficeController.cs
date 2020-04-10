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
        var result = await officeService.FindAsync();

        return Ok(result);
      }

      [HttpPost("offices")]
        public async Task<ActionResult<Office>> PostOffices([FromBody] Office office)
        {
            var result = await officeService.CreateAsync(office);
            return Ok(result);
        }

        

        [HttpGet("offices/{officeGuid}")]
        public async Task<ActionResult<Office>> GetOfficeId([FromRoute] Guid officeGuid)
        {
            var result = await officeService.GetAsync(officeGuid);
            return Ok(result);
        }

   

      [HttpPut("offices/{officeGuid}")]
        public async Task<ActionResult<Office>> PutOfficeId(
          [FromRoute] Guid officeGuid,
          [FromBody] Office target)

        {
            var source = await officeService.GetAsync(officeGuid);
            if (source == null)
            {
                // TODO создать ошибку для Not Found
                throw new NotImplementedException();
            }
            var result = await officeService.UpdateAsync(source, target);
            return Ok(result);
        }

        [HttpDelete("offices/{officeGuid}")]
        public async Task<ActionResult> DeleteOffice([FromRoute] Guid officeGuid)
        {
            var source = await officeService.GetAsync(officeGuid);
            if (source != null)
            {
                await officeService.DeleteAsync(source);
            }

            return Ok();
        }



      #endregion

  }
}