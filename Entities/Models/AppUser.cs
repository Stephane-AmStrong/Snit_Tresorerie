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
            Operations = new HashSet<Operation>();
        }

        public string ImgLink { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Guid? SiteId { get; set; }
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
