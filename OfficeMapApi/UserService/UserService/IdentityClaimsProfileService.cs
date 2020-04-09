using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Database.Entities;

namespace UserService
{
  public class IdentityClaimsProfileService : ProfileService<DbUser>
  {
    public IdentityClaimsProfileService(
      [FromServices]UserManager<DbUser> userManager,
      [FromServices]IUserClaimsPrincipalFactory<DbUser> claimsFactory) : base(userManager, claimsFactory)
    {
    }
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      var sub = context.Subject.GetSubjectId();
      var user = await UserManager.FindByIdAsync(sub);
      var principal = await ClaimsFactory.CreateAsync(user);
      var roles = await UserManager.GetRolesAsync(user);
      var claims = principal.Claims.ToList();
      claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
      claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));
      claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
      foreach (string role in roles)
      {
        claims.Add(new Claim(JwtClaimTypes.Role, role));
      }

      context.IssuedClaims = claims;
    }

    public override async Task IsActiveAsync(IsActiveContext context)
    {
      var sub = context.Subject.GetSubjectId();
      var user = await UserManager.FindByIdAsync(sub);
      context.IsActive = user != null;
    }
  }
}
