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

            new ClaimWrapper("paymentType.read.policy", "paymentType.read", "Read PaymentTypes"),
            new ClaimWrapper("paymentType.write.policy", "paymentType.write", "Write PaymentTypes"),
            new ClaimWrapper("paymentType.manage.policy", "paymentType.manage", "Manage PaymentTypes"),
            
            new ClaimWrapper("site.read.policy", "site.read", "Read Sites"),
            new ClaimWrapper("site.write.policy", "site.write", "Write Sites"),
            new ClaimWrapper("site.manage.policy", "site.manage", "Manage Sites"),

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

            new ClaimWrapper("appUser.read.policy", "appuser.read", "Read AppUsers"),
            new ClaimWrapper("appUser.write.policy", "appuser.write", "Write AppUsers"),
            new ClaimWrapper("appUser.manage.policy", "appuser.manage", "Manage AppUsers"),

            new ClaimWrapper("paymentType.read.policy", "paymentType.read", "Read PaymentTypes"),
            new ClaimWrapper("paymentType.write.policy", "paymentType.write", "Write PaymentTypes"),
            new ClaimWrapper("paymentType.manage.policy", "paymentType.manage", "Manage PaymentTypes"),

            new ClaimWrapper("site.read.policy", "site.read", "Read Sites"),
            new ClaimWrapper("site.write.policy", "site.write", "Write Sites"),
            new ClaimWrapper("site.manage.policy", "site.manage", "Manage Sites"),

            new ClaimWrapper("transaction.read.policy", "transaction.read", "Read Transaction"),
            new ClaimWrapper("transaction.write.policy", "transaction.write", "Write Transaction"),
            new ClaimWrapper("transaction.manage.policy", "transaction.manage", "Manage Transaction"),

            new ClaimWrapper("role.read.policy", "role.read", "Read Roles"),
            new ClaimWrapper("role.write.policy", "role.write", "Write Roles"),
            new ClaimWrapper("role.manage.policy", "role.manage", "Manage Roles"),
        };
    }
}
