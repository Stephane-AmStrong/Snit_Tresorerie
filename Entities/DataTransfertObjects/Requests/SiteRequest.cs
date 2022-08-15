using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class SiteRequest
    {
        public Guid? Id { get; set; }

        [Required, Display(Name = "Nom")]
        public string Name { get; set; }

        [Required, Display(Name = "Pays")]
        public string Country { get; set; }

        [Required, Display(Name = "Quartier général")]
        public string Headquarters { get; set; }

        public string AppUserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}
