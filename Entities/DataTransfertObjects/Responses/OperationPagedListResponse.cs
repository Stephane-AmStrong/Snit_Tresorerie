using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record OperationPagedListResponse : IComparable<OperationResponse>
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Libellé")]
        public string Name { get; set; }

        [Required, BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required, Display(Name = "Réference")]
        public string Reference { get; set; }

        [Required, Display(Name = "Type de operation")]
        public string Type { get; set; }

        [Required, Display(Name = "Montant hors taxes")]
        public int AmountBeforeTax { get; set; }

        [Required, Display(Name = "TVA")]
        public int VAT { get; set; }

        [Required, Display(Name = "TTC")]
        public int ATI { get; set; }

        [Required, Display(Name = "Intervenant")]
        public Guid IntervenorId { get; set; }

        public string AppUserId { get; set; }

        [Required, Display(Name = "Mode de paiement")]
        public Guid PaymentOptionId { get; set; }

        [Required, Display(Name = "Site")]
        public Guid SiteId { get; set; }

        [Required, Display(Name = "OperationType")]
        public Guid OperationTypeId { get; set; }

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
