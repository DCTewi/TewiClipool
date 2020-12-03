using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace TewiClipool.Client.Services
{
    public class RolesAccountClaimsPrincipalFactory
        : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public RolesAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }

        public override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var roles = account?.AdditionalProperties["role"] as JsonElement?;

            if (roles?.ValueKind == JsonValueKind.Array)
            {
                account.AdditionalProperties.Remove("role");
                var claimsPrincipal = base.CreateUserAsync(account, options).Result;

                foreach (var ele in roles.Value.EnumerateArray())
                {
                    (claimsPrincipal.Identity as ClaimsIdentity)
                        .AddClaim(new Claim("role", ele.GetString()));
                }
                return new ValueTask<ClaimsPrincipal>(claimsPrincipal);
            }

            return base.CreateUserAsync(account, options);
        }
    }
}
