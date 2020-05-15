using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(
            [FromServices] UserManager<DbUser> userManager,
            [FromServices] IMapper mapper)
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
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.Employee)
                .Where(u => u.Employee != null && u.Employee.Obsolete == false)
                .Select(u => new User()
                {
                  UserGuid = Guid.Parse(u.Id),
                  Email = u.Email,
                  Employee = new Employee
                  {
                    FirstName = u.Employee.FirstName,
                    SecondName = u.Employee.SecondName
                  }
                })
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<User>>(users));
        }

        /// <summary>
        /// Returns user by userGuid.
        /// </summary>
        /// <response code="200">Successfully returned user.</response>
        /// <response code="204">User not found.</response>
        [HttpGet("users/{userGuid}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(204)]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] Guid userGuid)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)
                .SingleOrDefaultAsync(u => u.Id == userGuid.ToString());

            if (user == null)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<User>(user));
        }

        /// <summary>
        /// Deletes the specified user by userGuid.
        /// </summary>
        /// <response code="200">Successfully deleted user.</response>
        /// <response code="204">User not found.</response>
        /// <response code="400">Bad request.</response>
        [HttpDelete("users/{userGuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userGuid)
        {
            var user = await _userManager.FindByIdAsync(userGuid.ToString());

            if (user == null)
            {
                return NoContent();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Updates user fields.
        /// </summary>
        /// <response code="204">Successfully updated user.</response>
        /// <response code="204">User not found.</response>
        /// <response code="400">Bad request.</response>
        [HttpPut("users/{userGuid}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> PutUser([FromRoute] Guid userGuid, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }

            var dbUser = await _userManager.Users
                .Include(u => u.Employee)
                .SingleOrDefaultAsync(u => u.Id == userGuid.ToString());

            if (dbUser == null)
            {
                return NoContent();
            }

            if (user.Employee != null)
            {
                dbUser.Employee.FirstName = user.Employee.FirstName;
                dbUser.Employee.SecondName = user.Employee.SecondName;
            }

            var result = await _userManager.UpdateAsync(dbUser);

            if (result.Succeeded)
            {
                var updatedUser = await _userManager.Users
                .Include(u => u.Employee)
                .SingleOrDefaultAsync(u => u.Id == dbUser.Id);

                return Ok(_mapper.Map<User>(updatedUser));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Creates user.
        /// </summary>
        /// <response code="204">Successfully created user.</response>
        /// <response code="400">Bad request.</response>
        [HttpPost("users")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> PostUser([FromBody] RegisterUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }

            var newUser = _mapper.Map<DbUser>(user);
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByEmailAsync(user.Email);

                return Ok(_mapper.Map<User>(createdUser));
            }

            return BadRequest(result.Errors);
        }
    }
}