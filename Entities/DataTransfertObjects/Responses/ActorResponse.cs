using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record ActorResponse
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }

        [Required, Display(Name = "Prénoms")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required, Display(Name = "Compte bancaire")]
        public string BankAccount { get; set; }

        [Required]
        public string Type { get; set; }


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
