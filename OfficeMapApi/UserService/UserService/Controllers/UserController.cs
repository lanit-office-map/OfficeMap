using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Response;
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
    private readonly Services.UserService userService;
    private readonly IMapper autoMapper;

    public UserController(
        [FromServices] Services.UserService userService,
        [FromServices] IMapper autoMapper)
    {
      this.userService = userService;
      this.autoMapper = autoMapper;
    }

    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <response code="200">Successfully returned users.</response>
    [HttpGet("users")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
      var response = await userService.FindAllAsync();

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    /// <summary>
    /// Returns user by userGuid.
    /// </summary>
    /// <response code="200">Successfully returned user.</response>
    /// <response code="204">User not found.</response>
    [HttpGet("users/{userGuid}")]
    [Authorize]
    public async Task<IActionResult> GetUser([FromRoute] Guid userGuid)
    {
      var response = await userService.GetAsync(userGuid);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    /// <summary>
    /// Deletes the specified user by userGuid.
    /// </summary>
    /// <response code="200">Successfully deleted user.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Bad request.</response>
    [HttpDelete("users/{userGuid}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userGuid)
    {
      await userService.DeleteAsync(userGuid);
      return NoContent();
    }

    /// <summary>
    /// Updates user fields.
    /// </summary>
    /// <response code="204">Successfully updated user.</response>
    /// <response code="204">User not found.</response>
    /// <response code="400">Bad request.</response>
    [HttpPut("users/{userGuid}")]
    [Authorize]
    public async Task<IActionResult> PutUser(
      [FromRoute] Guid userGuid,
      [FromBody] User target)
    {
      target.UserGuid = userGuid;
      var response = await userService.UpdateAsync(target);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }

    /// <summary>
    /// Creates user.
    /// </summary>
    /// <response code="204">Successfully created user.</response>
    /// <response code="400">Bad request.</response>
    [HttpPost("users")]
    [Authorize]
    public async Task<IActionResult> PostUser([FromBody] RegisterUserModel user)
    {
      var response = await userService.CreateAsync(user);

      return response.Status == ResponseResult.Success
        ? Ok(response.Result)
        : StatusCode((int)response.Error.StatusCode, response.Error.Message);
    }
  }
}