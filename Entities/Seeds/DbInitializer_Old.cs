using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Seeds
{
    public class DbInitializer_Old
    {
        //public static void SeedDefaultRoles(RepositoryContext context)
        //{
        //    context.Database.EnsureCreated();


        //    if (!context.Roles.Any(x => x.Name == "SuperAdmin"))
        //    {
        //        var superAdminRole = new IdentityRole
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Name = "SuperAdmin",
        //            NormalizedName = "SUPERADMIN"
        //        };

        //        context.Roles.Add(superAdminRole);

        //        ClaimsStore.AllClaims.ForEach(claim =>
        //        {
        //            var claimWrapper = new ClaimWrapper();
        //            claimWrapper.RoleId = superAdminRole.Id;
        //            claimWrapper.InitializeFromClaim(claim);

        //            context.RoleClaims.Add(claimWrapper);
        //        });
        //    }
            
        //    if (!context.Roles.Any(x => x.Name == "Admin"))
        //    {
        //        var AdminRole = new IdentityRole
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Name = "Admin",
        //            NormalizedName = "ADMIN"
        //        };

        //        context.Roles.Add(AdminRole);

        //        ClaimsStore.AdministratorClaims.ForEach(claim =>
        //        {
        //            var claimWrapper = new ClaimWrapper();
        //            claimWrapper.RoleId = AdminRole.Id;
        //            claimWrapper.InitializeFromClaim(claim);

        //            context.RoleClaims.Add(claimWrapper);
        //        });
        //    }

        //    context.SaveChanges();
        //}
    }
}
