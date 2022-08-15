using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public record Site
    {
        public Site()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Headquarters { get; set; }
        [Required]
        public string AppUserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
