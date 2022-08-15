using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record TransactionPagedListResponse : IComparable<TransactionResponse>
    {
        public Guid Id { get; set; }
        [Required, BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required, Display(Name = "Réference")]
        public string Reference { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Nature { get; set; }

        [Required, Display(Name = "Montant hors taxes")]
        public int AmountBeforeTax { get; set; }

        [Required, Display(Name = "TVA")]
        public int VAT { get; set; }

        [Required, Display(Name = "TTC")]
        public int ATI { get; set; }

        [Required, Display(Name = "Mode de paiement")]
        public string ModeOfPayment { get; set; }

        public Guid ActorId { get; set; }
        public string AppUserId { get; set; }
        public Guid SiteId { get; set; }

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
