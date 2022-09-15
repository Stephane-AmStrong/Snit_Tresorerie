using Entities.ViewModels;

namespace Repository.Seeds
{
    public static class ClaimsStore
    {
        public static readonly List<ClaimViewModel> AllClaims = new()
        {
            new ClaimViewModel("intervenor.read.policy", "intervenor.read", "Lecture des Intervenants"),
            new ClaimViewModel("intervenor.write.policy", "intervenor.write", "Écriture des Intervenants"),
            new ClaimViewModel("intervenor.manage.policy", "intervenor.manage", "Gestion des Intervenants"),

            new ClaimViewModel("appUser.read.policy", "appuser.read", "Lecture des Utilisateurs"),
            new ClaimViewModel("appUser.write.policy", "appuser.write", "Écriture des Utilisateurs"),
            new ClaimViewModel("appUser.manage.policy", "appuser.manage", "Gestion des Utilisateurs"),

            new ClaimViewModel("paymentOption.read.policy", "paymentOption.read", "Lecture des Mode de Paiement"),
            new ClaimViewModel("paymentOption.write.policy", "paymentOption.write", "Écriture des Mode de Paiement"),
            new ClaimViewModel("paymentOption.manage.policy", "paymentOption.manage", "Gestion des Mode de Paiement"),
            
            new ClaimViewModel("site.read.policy", "site.read", "Lecture des Sites"),
            new ClaimViewModel("site.write.policy", "site.write", "Écriture des Sites"),
            new ClaimViewModel("site.manage.policy", "site.manage", "Gestion des Sites"),

            new ClaimViewModel("operation.read.policy", "operation.read", "Lecture des Opération"),
            new ClaimViewModel("operation.write.policy", "operation.write", "Écriture des Opération"),
            new ClaimViewModel("operation.manage.policy", "operation.manage", "Gestion des Opération"),

            new ClaimViewModel("operationType.read.policy", "operationType.read", "Lecture des Type d'Opération"),
            new ClaimViewModel("operationType.write.policy", "operationType.write", "Écriture des Type d'Opération"),
            new ClaimViewModel("operationType.manage.policy", "operationType.manage", "Gestion des Type d'Opération"),

            new ClaimViewModel("role.read.policy", "role.read", "Lecture des Roles"),
            new ClaimViewModel("role.write.policy", "role.write", "Écriture des Roles"),
            new ClaimViewModel("role.manage.policy", "role.manage", "Gestion des Roles"),
        };

        public static readonly List<ClaimViewModel> AdministratorClaims = new()
        {
            new ClaimViewModel("intervenor.read.policy", "intervenor.read", "Lecture des Intervenants"),
            new ClaimViewModel("intervenor.write.policy", "intervenor.write", "Écriture des Intervenants"),
            new ClaimViewModel("intervenor.manage.policy", "intervenor.manage", "Gestion des Intervenants"),

            new ClaimViewModel("appUser.read.policy", "appuser.read", "Lecture des Utilisateurs"),
            new ClaimViewModel("appUser.write.policy", "appuser.write", "Écriture des Utilisateurs"),
            new ClaimViewModel("appUser.manage.policy", "appuser.manage", "Gestion des Utilisateurs"),

            new ClaimViewModel("paymentOption.read.policy", "paymentOption.read", "Lecture des Mode de Paiement"),
            new ClaimViewModel("paymentOption.write.policy", "paymentOption.write", "Écriture des Mode de Paiement"),
            new ClaimViewModel("paymentOption.manage.policy", "paymentOption.manage", "Gestion des Mode de Paiement"),

            new ClaimViewModel("site.read.policy", "site.read", "Lecture des Sites"),
            new ClaimViewModel("site.write.policy", "site.write", "Écriture des Sites"),
            new ClaimViewModel("site.manage.policy", "site.manage", "Gestion des Sites"),

            new ClaimViewModel("operation.read.policy", "operation.read", "Lecture des Opération"),
            new ClaimViewModel("operation.write.policy", "operation.write", "Écriture des Opération"),
            new ClaimViewModel("operation.manage.policy", "operation.manage", "Gestion des Opération"),

            new ClaimViewModel("operationType.read.policy", "operationType.read", "Lecture des Type d'Opération"),
            new ClaimViewModel("operationType.write.policy", "operationType.write", "Écriture des Type d'Opération"),
            new ClaimViewModel("operationType.manage.policy", "operationType.manage", "Gestion des Type d'Opération"),

            new ClaimViewModel("role.read.policy", "role.read", "Lecture des Roles"),
            new ClaimViewModel("role.write.policy", "role.write", "Écriture des Roles"),
            new ClaimViewModel("role.manage.policy", "role.manage", "Gestion des Roles"),
        };
    }
}
