using Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public class AppUserRequest
    {
        public string Id { get; set; }
        [Display(Name = "Image")]
        public string ImgLink { get; set; }

        [Required, Display(Name = "Prénoms")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Nom")]
        public string LastName { get; set; }
        [Required, Display(Name = "Role")]
        public string RoleId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirmation mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne concordent pas.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Site")]
        public Guid? SiteId { get; set; }
    }
}
