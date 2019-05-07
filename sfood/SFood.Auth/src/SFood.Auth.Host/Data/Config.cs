using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace SFood.Auth.Host
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            var claimTypes = new[]
            {
                JwtClaimTypes.Role,
                "resid"
            };

            return new[]
            {
                new ApiResource("merchant.api", "Merchant API", claimTypes)
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            var clients = new[]
            {
                new Client
                {
                    ClientId = "merchantMobile",
                    ClientName = "Merchant Mobile",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret(@"secret".Sha256())},

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        "merchant.api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };

            return clients;
        }
    }
}
