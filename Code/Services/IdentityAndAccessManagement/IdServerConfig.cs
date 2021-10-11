using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement
{
    /// <summary>
    /// TODO To Be removed
    /// </summary>
    public class IdServerConfig
    {
        string _uiUrl;
        public IdServerConfig(string uiUrl)
        {
            _uiUrl = uiUrl;
        }
        public IEnumerable<Client> GetClients()
        {
            Client client = new Client()
            {
                ClientId = "BSKonnectIdentityServerID",
                ClientName = "BS Konnect",
                AllowedGrantTypes = GrantTypes.Implicit,
                ClientSecrets = new List<Secret> { new Secret("BSKonnectPassword@123".Sha256()) },
                RedirectUris = { $"{_uiUrl}/signinredirect" },
                PostLogoutRedirectUris = { $"{_uiUrl}/signoutredirect" },
                AllowAccessTokensViaBrowser = true,
                AllowedScopes = { "openid", "profile", "email" },
                AllowedCorsOrigins = { _uiUrl },
                AlwaysIncludeUserClaimsInIdToken = true,
                IdentityTokenLifetime = 3600
            };

            return new List<Client>() { client };
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
        };
    }
}
