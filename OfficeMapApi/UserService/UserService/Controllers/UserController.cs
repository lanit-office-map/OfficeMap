using System;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserController(
            UserManager<DbUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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