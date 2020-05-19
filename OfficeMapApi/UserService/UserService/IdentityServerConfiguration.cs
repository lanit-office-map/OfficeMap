using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace UserService
{
    /// <summary>
    /// IdentityServer4 configuration.
    /// </summary>
    public class IdentityServerConfiguration
  {
    #region public properties
    public IdentityResource[] IdentityResources { get; }

    public  ApiResource[] ApiResources { get; }

    public  Client[] Clients { get;}
    #endregion

    #region public methods
    public IdentityServerConfiguration(IConfiguration configuration)
    {
      IdentityResources = new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Email()
      };
      ApiResources = new[]
      {
        new ApiResource("UserService"),
        new ApiResource("OfficeService"),
        new ApiResource("SpaceService"),
        new ApiResource("WorkplaceService"),
      };

      var redirectUri =
        $"{configuration["Addresses:Frontend:OfficeMapUI"]}/auth-callback";
      var postLogoutRedirectUri =
        $"{configuration["Addresses:Frontend:OfficeMapUI"]}/home";

      Clients = new[]
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
          RedirectUris = {redirectUri},
          PostLogoutRedirectUris = {postLogoutRedirectUri},
          AllowedScopes = new List<string>
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Email,
            "UserService",
            "OfficeService",
            "SpaceService",
            "WorkplaceService"
          },
          AllowedCorsOrigins =
            {configuration["Addresses:Frontend:OfficeMapUI"]},
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
    }
    #endregion
  }
}
