using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using UserService.Models;
using UserService.Database.Entities;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers
{
    [Route("UserService/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<DbUser> _signInManager;
        private readonly UserManager<DbUser> _userManager;

        /// <param name="userManager">Allows you to manage users.</param>
        /// <param name="signInManager">Provides the APIs for user sign in.</param>
        public AccountController(
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

    /// <summary>
    /// Attempts to sign in the email of user and password combination.
    /// </summary>
    /// <param name="model">Contains mail, password, and whether to remember.</param>
    /// <returns></returns>
    /// <response code="200">Successfully login.</response>
    /// <response code="204">User is not logged in.</response>
    /// <response code="400">Invalid fields.</response>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody]LoginUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(
             user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
              return Redirect(model.ReturnUrl);
            }

            return NoContent();
        }

        /// <summary>
        /// Attempts to sign out the user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Successfully logout.</response>
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        ///  Attempts to change password for current user.
        /// </summary>
        /// <param name="model">Contains new password, old password, and whether to remember (trying to re-login).</param>
        /// <returns></returns>
        /// <response code="204">Successfully changed password.</response>
        /// <response code="400">Invalid fields.</response>
        /// <response code="401">User not authorized.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(DbUser), 400)]
        [ProducesResponseType(401)]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.ChangePasswordAsync(
                user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        ///  Attempts to change email for current user.
        /// </summary>
        /// <param name="model">Contains new email and whether to remember (trying to re-login).</param>
        /// <returns></returns>
        /// <response code="204">Successfully changed email.</response>
        /// <response code="400">Invalid fields.</response>
        /// <response code="401">User not authorized.</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(DbUser), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return Ok();
            }

            return BadRequest();
        }
    }
}