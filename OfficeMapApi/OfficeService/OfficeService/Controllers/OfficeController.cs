using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Models;
using OfficeService.Services.Interface;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Common.Response;

namespace OfficeService.Controllers
{
    [Route("[controller]")]
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
        public async Task<ActionResult<IEnumerable<OfficeResponse>>> GetOffices()
        {
            var response = await officeService.FindAllAsync();

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpPost("offices")]
        public async Task<ActionResult<OfficeResponse>> PostOffice([FromBody] Office office)
        {
            var response = await officeService.CreateAsync(office);

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpGet("offices/{officeGuid}")]
        public async Task<ActionResult<OfficeResponse>> GetOffice([FromRoute] Guid officeGuid)
        {
            var response = await officeService.GetAsync(officeGuid);

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpPut("offices/{officeGuid}")]
        public async Task<ActionResult<OfficeResponse>> PutOffice(
            [FromRoute] Guid officeGuid,
            [FromBody] Office target)

        {
            target.OfficeGuid = officeGuid;
            var response = await officeService.UpdateAsync(target);

            return response.Status == ResponseResult.Success
            ? Ok(response.Result)
            : StatusCode((int)response.Error.StatusCode, response.Error.Message);
        }

        [HttpDelete("offices/{officeGuid}")]
        public async Task<ActionResult> DeleteOffice([FromRoute] Guid officeGuid)
        {
            await officeService.DeleteAsync(officeGuid);
            return NoContent(); ;
        }
        #endregion
    }
}