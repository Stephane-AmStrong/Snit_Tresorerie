using Entities.DataTransfertObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record AppUserPagedListResponse
    {
        public string Id { get; set; }
        [Display(Name = "Image")]
        public string ImgLink { get; set; }
        [Display(Name = "Prénoms")]
        public string FirstName { get; set; }
        [Display(Name = "Nom")]
        public string LastName { get; set; }
        [Display(Name = "E.mail")]
        public string Email { get; set; }
        [Display(Name = "Site")]
        public Guid? SiteId { get; set; }
    }
}
