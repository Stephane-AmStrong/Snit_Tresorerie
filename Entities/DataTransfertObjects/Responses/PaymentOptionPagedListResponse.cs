using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record PaymentOptionPagedListResponse
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Nom")]
        public string Name { get; set; }

        [Required, Display(Name = "Description")]
        public string Description { get; set; }


        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
