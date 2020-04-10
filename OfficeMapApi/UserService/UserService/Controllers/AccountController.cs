using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Database.Entities;
using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Attempts to sign in the email of user and password combination
        /// </summary>
        /// <param name="model">Contains mail, password, and whether to remember</param>
        /// <returns></returns>
        /// <response code="204">Ok</response>
        /// <response code="400">Invalid fields</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(DbUser), 400)]
        [ProducesResponseType(404)]
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
                return NoContent();
            }

            return NotFound();
        }

        /// <summary>
        /// Attempts to sign out the user
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Ok</response>
        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        /// <summary>
        ///  Attempts to change password for current user
        /// </summary>
        /// <param name="model">Contains new password, old password, and whether to remember (trying to re-login)</param>
        /// <returns></returns>
        /// <response code="204">Ok</response>
        /// <response code="400">Invalid fields</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(DbUser), 400)]
        [ProducesResponseType(404)]
        [HttpPost]
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
                    return NoContent();
                }

                return BadRequest();
            }

            return NotFound();
        }

        /// <summary>
        ///  Attempts to change email for current user
        /// </summary>
        /// <param name="model">Contains new email and whether to remember (trying to re-login)</param>
        /// <returns></returns>
        /// <response code="204">Ok</response>
        /// <response code="400">Invalid fields</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(DbUser), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
                var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return NoContent();
                }

                return BadRequest();
            }

            return NotFound();
        }
    }
}