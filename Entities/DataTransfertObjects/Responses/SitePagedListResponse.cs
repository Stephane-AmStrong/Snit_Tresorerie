using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record SitePagedListResponse
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Nom")]
        public string Name { get; set; }

        [Required, Display(Name = "Pays")]
        public string Country { get; set; }

        [Required, Display(Name = "Quartier général")]
        public string Headquarters { get; set; }
    }
}
