using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Transactions = new HashSet<Transaction>();
        }

        public string ImgLink { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
