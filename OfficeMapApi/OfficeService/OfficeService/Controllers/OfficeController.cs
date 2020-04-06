using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeService.Models;
using OfficeService.Services.Interface;

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
        var result = await officeService.GetOfficesAsync();

        return Ok(result);
      }
      #endregion

  }
}