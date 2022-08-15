using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public record Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; }
        public string Nature { get; set; }
        public int AmountBeforeTax { get; set; }
        public int VAT { get; set; }
        public int ATI { get; set; }
        public string ModeOfPayment { get; set; }

        public Guid ActorId { get; set; }
        public string AppUserId { get; set; }
        public Guid SiteId { get; set; }


        [ForeignKey("ActorId")]
        public virtual Actor Actor { get; set; }
        
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

    }
}
