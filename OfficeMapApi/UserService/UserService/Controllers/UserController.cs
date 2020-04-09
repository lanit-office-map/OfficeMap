using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Database.Entities;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<DbUser> _userManager;

        public UserController(
            UserManager<DbUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("users/{userGuid}")]
        [ProducesResponseType(typeof(DbUser), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(Guid userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid.ToString());

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpDelete("users/{userGuid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(503)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(Guid userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid.ToString());

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                return StatusCode(503);
            }

            return NotFound();
        }
    }
}