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
        new IdentityResources.OpenId()
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
        ClientId = "service.client",
        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
        ClientSecrets =
        {
          new Secret("secret".Sha256())
        },
        AllowOfflineAccess = true,
        AccessTokenLifetime =
          (int)TimeSpan.FromDays(1).TotalSeconds,
        AllowedScopes = new List<string>
        {
          "officemapapis"
        }
      }
    };
    #endregion
  }
}
