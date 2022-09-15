using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public record Intervenor
    {
        public Intervenor()
        {
            Operations = new HashSet<Operation>();
        }

        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public string Type { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
