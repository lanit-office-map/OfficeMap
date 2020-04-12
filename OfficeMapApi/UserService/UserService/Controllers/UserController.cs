using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Database.Entities;
using UserService.Models;

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

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <response code="200">Successfully returned users.</response>
        [HttpGet("users")]
        [ProducesResponseType(typeof(List<User>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();

            return Ok(_mapper.Map<User[]>(users));
        }

        /// <summary>
        /// Returns user by userGuid.
        /// </summary>
        /// <response code="200">Successfully returned user.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("users/{userGuid}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(Guid userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid.ToString());

            if (user != null)
            {
                return Ok(_mapper.Map<User>(user));
            }

            return NotFound();
        }

        /// <summary>
        /// Deletes the specified user by userGuid.
        /// </summary>
        /// <response code="204">Successfully deleted user.</response>
        /// <response code="404">User not found.</response>
        [HttpDelete("users/{userGuid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(Policy = "IsAdmin")]
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
            }

            return NotFound();
        }

        /// <summary>
        /// Updates user fields.
        /// </summary>
        /// <response code="204">Successfully updated user.</response>
        /// <response code="404">User not found.</response>
        /// <response code="400">Bad request.</response>
        [HttpPut("users/{userGuid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> PutUser([FromBody] User user, Guid userGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }

            var dbUser = await _userManager.FindByIdAsync(userGuid.ToString());

            if (dbUser != null)
            {
                var result = await _userManager.UpdateAsync(_mapper.Map<DbUser>(user));

                if (result.Succeeded)
                {
                    return NoContent();
                }
            }

            return NotFound();
        }
    }
}