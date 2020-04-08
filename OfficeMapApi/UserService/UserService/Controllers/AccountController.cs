using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using System;

namespace UserService.Controllers
{
    [Route("UserService/[controller]/[action]")]
    [ApiController, Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<DbUser> _signInManager;
        private readonly UserManager<DbUser> _userManager;

        /// <param name="userManager">Allows you to manage users</param>
        /// <param name="signInManager">Provides the APIs for user sign in</param>
        public AccountController(
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(
             user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var userId = Guid.Parse(User.FindFirst("sub").Value);
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(
                    user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return Ok();
                }

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel model, [FromQuery] string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token); //token???

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return Ok();
                }

                return BadRequest();
            }

            return BadRequest();
        }
    }
}