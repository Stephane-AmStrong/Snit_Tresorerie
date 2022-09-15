using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record SiteResponse
    {
        public Guid Id { get; set; }

        [Required, Display(Name = "Code")]
        public string Code { get; set; }

        [Required, Display(Name = "Libellé")]
        public string Name { get; set; }

        [Required, Display(Name = "Pays")]
        public string Country { get; set; }

        [Required, Display(Name = "Ville")]
        public string City { get; set; }

        [Required, Display(Name = "Adresse")]
        public string Address { get; set; }

        [Required, Display(Name = "Téléphone 1")]
        public string Telephone1 { get; set; }

        [Display(Name = "Téléphone 2")]
        public string Telephone2 { get; set; }

        [Display(Name = "E.mail")]
        public string Email { get; set; }
        
        [Display(Name = "Utilisateur")]
        public string AppUserId { get; set; }

        [Display(Name = "Opérations")]
        public virtual OperationPagedListResponse[] Operations { get; set; }


        public override string ToString()
        {
            return $"{Name} {Country} {City}";
        }
    }
}
