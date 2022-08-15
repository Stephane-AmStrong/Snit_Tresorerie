using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Seeds
{
    public static class ClaimsStore
    {
        public static readonly List<ClaimWrapper> AllClaims = new()
        {
            new ClaimWrapper("actor.read.policy", "actor.read", "Read Actors"),
            new ClaimWrapper("actor.write.policy", "actor.write", "Write Actors"),
            new ClaimWrapper("actor.manage.policy", "actor.manage", "Manage Actors"),

            new ClaimWrapper("appUser.read.policy", "appuser.read", "Read AppUsers"),
            new ClaimWrapper("appUser.write.policy", "appuser.write", "Write AppUsers"),
            new ClaimWrapper("appUser.manage.policy", "appuser.manage", "Manage AppUsers"),


            new ClaimWrapper("site.read.policy", "site.read", "Read Categories"),
            new ClaimWrapper("site.write.policy", "site.write", "Write Categories"),
            new ClaimWrapper("site.manage.policy", "site.manage", "Manage Categories"),

            new ClaimWrapper("transaction.read.policy", "transaction.read", "Read Transaction"),
            new ClaimWrapper("transaction.write.policy", "transaction.write", "Write Transaction"),
            new ClaimWrapper("transaction.manage.policy", "transaction.manage", "Manage Transaction"),

            new ClaimWrapper("role.read.policy", "role.read", "Read Roles"),
            new ClaimWrapper("role.write.policy", "role.write", "Write Roles"),
            new ClaimWrapper("role.manage.policy", "role.manage", "Manage Roles"),
        };

        public static readonly List<ClaimWrapper> AdministratorClaims = new()
        {
            new ClaimWrapper("actor.read.policy", "actor.read", "Read Actors"),
            new ClaimWrapper("actor.write.policy", "actor.write", "Write Actors"),
            new ClaimWrapper("actor.manage.policy", "actor.manage", "Manage Actors"),

            new ClaimWrapper("appUser.read.policy", "appUser.read", "Read AppUsers"),
            new ClaimWrapper("appUser.write.policy", "appUser.write", "Write AppUsers"),
            new ClaimWrapper("appUser.manage.policy", "appUser.manage", "Manage AppUsers"),


            new ClaimWrapper("site.read.policy", "site.read", "Read Categories"),
            new ClaimWrapper("site.write.policy", "site.write", "Write Categories"),
            new ClaimWrapper("site.manage.policy", "site.manage", "Manage Categories"),

            new ClaimWrapper("transaction.read.policy", "transaction.read", "Read Transaction"),
            new ClaimWrapper("transaction.write.policy", "transaction.write", "Write Transaction"),
            new ClaimWrapper("transaction.manage.policy", "transaction.manage", "Manage Transaction"),

            new ClaimWrapper("role.read.policy", "role.read", "Read Roles"),
            new ClaimWrapper("role.write.policy", "role.write", "Write Roles"),
            new ClaimWrapper("role.manage.policy", "role.manage", "Manage Roles"),
        };
    }
}
