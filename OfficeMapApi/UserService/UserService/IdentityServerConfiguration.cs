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
        new ApiResource("UserService"),
        new ApiResource("OfficeService"),
        new ApiResource("SpaceService"),
        new ApiResource("WorkplaceService"),
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
        RedirectUris = { "https://digitalofficemap.azurewebsites.net/auth-callback" },
        PostLogoutRedirectUris = { "https://digitalofficemap.azurewebsites.net/home" },
        AllowedScopes = new List<string>
        {
          IdentityServerConstants.StandardScopes.OpenId,
          IdentityServerConstants.StandardScopes.Email,
          "UserService",
          "OfficeService",
          "SpaceService",
          "WorkplaceService"
        },
        AllowedCorsOrigins = { "https://digitalofficemap.azurewebsites.net" },
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
        AllowedScopes =
        {
          "UserService",
          "OfficeService",
          "SpaceService",
          "WorkplaceService"
        }
      }
    };
    #endregion
  }
}
