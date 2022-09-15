using Entities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required, Display(Name ="Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Display(Name ="Se rappeler de moi")]
        public bool RememberMe { get; set; } = false;
    }
}
