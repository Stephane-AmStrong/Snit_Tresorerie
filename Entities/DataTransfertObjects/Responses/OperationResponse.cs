using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record OperationResponse : IComparable<OperationResponse>
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Libellé")]
        public string Name { get; set; }
        [Required, BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required, Display(Name = "Réference")]
        public string Reference { get; set; }

        [Required, Display(Name = "Type d' opération")]
        public string Type { get; set; }

        [Required, Display(Name = "Montant hors taxes")]
        public int AmountBeforeTax { get; set; }

        [Required, Display(Name = "TVA")]
        public int VAT { get; set; }

        [Required, Display(Name = "TTC")]
        public int ATI { get; set; }

        [Required, Display(Name = "Mode de paiement")]
        public Guid PaymentOptionId { get; set; }

        [Required, Display(Name = "Intervenant")]
        public Guid IntervenorId { get; set; }

        public string AppUserId { get; set; }

        [Required, Display(Name = "Site")]
        public Guid SiteId { get; set; }

        [Required, Display(Name = "Type d'Opération")]
        public Guid OperationTypeId { get; set; }



        [Display(Name = "Intervenant")]
        public virtual IntervenorResponse Intervenor { get; set; }

        [Display(Name = "Utilisateur")]
        public virtual AppUserResponse AppUser { get; set; }

        [Display(Name = "Mode de paiement")]
        public virtual PaymentOptionResponse PaymentOption { get; set; }

        [Display(Name = "Site")]
        public virtual SiteResponse Site { get; set; }

        [Display(Name = "Type d'Opération")]
        public virtual OperationTypeResponse OperationType { get; set; }

        public int CompareTo(OperationResponse otherOperationResponse)
        {
            if (Id == otherOperationResponse.Id)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
