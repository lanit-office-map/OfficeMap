using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Database.Entities;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

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

        [HttpPost, Authorize]
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