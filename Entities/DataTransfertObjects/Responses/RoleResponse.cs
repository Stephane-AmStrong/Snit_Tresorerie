
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record RoleResponse
    {
        public string Id { get; set; }
        
        [Display(Name = "Nom")]
        public string Name { get; set; }
        
        public string NormalizedName { get; set; }
        
        [Display(Name = "Revendications")]
        public IList<ClaimResponse> Claims { get; set; }
        
        public virtual AppUserResponse[] AppUsers { get; set; }
    }
}
