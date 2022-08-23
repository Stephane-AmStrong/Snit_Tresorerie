using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public record Transaction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; }
        public int AmountBeforeTax { get; set; }
        public int VAT { get; set; }
        public int ATI { get; set; }

        public Guid ActorId { get; set; }
        public string AppUserId { get; set; }
        public Guid PaymentTypeId { get; set; }
        public Guid SiteId { get; set; }


        [ForeignKey("ActorId")]
        public virtual Actor Actor { get; set; }
        
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
        
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

    }
}
