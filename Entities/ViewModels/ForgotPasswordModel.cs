using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
