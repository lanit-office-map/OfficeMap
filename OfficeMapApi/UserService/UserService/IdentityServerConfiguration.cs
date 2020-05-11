using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace UserService
{
    /// <summary>
    /// IdentityServer4 configuration.
    /// </summary>
    public static class IdentityServerConfiguration
  {
    #region public properties

    public static IdentityResource[] IdentityResources => new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Email()
      };


    public static ApiResource[] ApiResources => new[]
    {
        new ApiResource
        {
          Name = "OfficeMapAPIs",
          Scopes = new List<Scope>
          {
            new Scope
            {
              Name = "officemapapis",
              DisplayName = "Access to OfficeMap APIs"
            }
          }
        }
      };

    public static Client[] Clients => new []
    {
      new Client
      {
        ClientId = "angular.client",
        AllowedGrantTypes = GrantTypes.Implicit,
        ClientSecrets =
        {
          new Secret("secret".Sha256())
        },
        AllowOfflineAccess = true,
        AccessTokenLifetime =
          (int)TimeSpan.FromDays(1).TotalSeconds,
        RedirectUris = { "http://localhost:4200/auth-callback" },
        PostLogoutRedirectUris = { "http://localhost:4200/home" },
        AllowedScopes = new List<string>
        {
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Email,
          "officemapapis"
        },
        AllowedCorsOrigins = { "http://localhost:4200" },
        AllowAccessTokensViaBrowser = true,
        //AlwaysIncludeUserClaimsInIdToken = true,
        RequireConsent = false,
      },
      new Client
      {
        ClientId = "service.client",
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets =
        {
          new Secret("secret".Sha256())
        },
        AllowOfflineAccess = true,
        AccessTokenLifetime =
          (int)TimeSpan.FromDays(1).TotalSeconds,
        AllowedScopes = { "officemapapis"}
      }
    };
    #endregion
  }
}
