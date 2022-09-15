
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class RoleRequest
    {
        public string Id { get; set; }
        [Required, Display(Name="Nom")]
        public string Name { get; set; }
    }
}
