using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record TransactionResponse : IComparable<TransactionResponse>
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Libellé")]
        public string Name { get; set; }
        [Required, BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required, Display(Name = "Réference")]
        public string Reference { get; set; }

        [Required, Display(Name = "Type de transaction")]
        public string Type { get; set; }

        [Required, Display(Name = "Montant hors taxes")]
        public int AmountBeforeTax { get; set; }

        [Required, Display(Name = "TVA")]
        public int VAT { get; set; }

        [Required, Display(Name = "TTC")]
        public int ATI { get; set; }

        [Required, Display(Name = "Mode de paiement")]
        public Guid PaymentTypeId { get; set; }

        [Required, Display(Name = "Acteur")]
        public Guid ActorId { get; set; }
        public string AppUserId { get; set; }
        [Required, Display(Name = "Site")]
        public Guid SiteId { get; set; }


        public virtual ActorResponse Actor { get; set; }
        public virtual AppUserResponse AppUser { get; set; }
        public virtual PaymentTypeResponse PaymentType { get; set; }
        public virtual SiteResponse Site { get; set; }

        public int CompareTo(TransactionResponse otherTransactionResponse)
        {
            if (Id == otherTransactionResponse.Id)
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
