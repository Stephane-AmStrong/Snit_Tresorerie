using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public record Operation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; }
        public int AmountBeforeTax { get; set; }
        public int VAT { get; set; }
        public int ATI { get; set; }

        public Guid IntervenorId { get; set; }
        public string AppUserId { get; set; }
        public Guid PaymentOptionId { get; set; }
        public Guid OperationTypeId { get; set; }
        public Guid SiteId { get; set; }


        [ForeignKey("IntervenorId")]
        public virtual Intervenor Intervenor { get; set; }
        
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
        
        [ForeignKey("PaymentOptionId")]
        public virtual PaymentOption PaymentOption { get; set; }
        
        [ForeignKey("OperationTypeId")]
        public virtual OperationType OperationType { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

    }
}
