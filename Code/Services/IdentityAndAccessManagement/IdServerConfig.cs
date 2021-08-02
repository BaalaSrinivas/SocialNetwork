using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement
{
    public class IdServerConfig
    {
        public static IEnumerable<Client> GetClients()
        {
            Client client = new Client()
            {
                ClientId = "BSKonnectIdentityServerID",
                ClientName = "BS Konnect",
                AllowedGrantTypes = GrantTypes.Implicit,
                ClientSecrets = new List<Secret> { new Secret("BSKonnectPassword@123".Sha256()) },
                RedirectUris = { "http://localhost:4200/signinredirect" },
                AllowAccessTokensViaBrowser = true,
                AllowedScopes = { "openid", "profile", "email" },
                AllowedCorsOrigins = { "http://localhost:4200" },
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
