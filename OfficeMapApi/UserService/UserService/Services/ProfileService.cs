using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserService.Database.Entities;

namespace UserService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<DbUser> userManager;

        public ProfileService(UserManager<DbUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Role, roles.Any() ? roles.First() : "User"),
                    new Claim(JwtClaimTypes.Role, roles.Any() ? roles.First() : "Admin")
                };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null && user.LockoutEnabled;
        }
    }
}
