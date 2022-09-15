using Repository.Seeds;

namespace Snit_Tresorerie_WebApp.Extensions
{
    public static class ClaimPolicyExtensions
    {
        public static void ConfigureClaimPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                {
                    option.AddPolicy(ClaimsStore.AllClaims[i].PolicyName, policy => policy.RequireClaim(ClaimsStore.AllClaims[i].Type));
                }
            });
        }
    }
}
