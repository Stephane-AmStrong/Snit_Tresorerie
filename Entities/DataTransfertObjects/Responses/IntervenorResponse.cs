using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record IntervenorResponse
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Image")]
        public string ImgLink { get; set; }

        [Required, Display(Name = "Prénoms")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required, Display(Name = "Compte bancaire")]
        public string BankAccount { get; set; }


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
