using Entities.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.Seeds
{
    public static class IdentityInitializer
    {
        public static async Task<WebApplication> SeedDefaultRolesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var superAdmin = await roleManager.FindByNameAsync(EnumRole.SuperAdmin.ToString());

                if (superAdmin == null)
                {
                    superAdmin = new IdentityRole(EnumRole.SuperAdmin.ToString());
                    await roleManager.CreateAsync(superAdmin);

                    for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                    {
                        await roleManager.AddClaimAsync(superAdmin, ClaimsStore.AllClaims[i]);
                    }
                }


                var admin = await roleManager.FindByNameAsync(EnumRole.Admin.ToString());

                if (admin == null)
                {
                    admin = new IdentityRole(EnumRole.Admin.ToString());
                    await roleManager.CreateAsync(admin);

                    for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                    {
                        await roleManager.AddClaimAsync(admin, ClaimsStore.AllClaims[i]);
                    }
                }


                var user = await roleManager.FindByNameAsync(EnumRole.User.ToString());

                if (user == null)
                {
                    user = new IdentityRole(EnumRole.User.ToString());
                    await roleManager.CreateAsync(user);

                    for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                    {
                        await roleManager.AddClaimAsync(user, ClaimsStore.AllClaims[i]);
                    }
                }
            }

            return webApp;
        }
    }
}
