using System.ComponentModel.DataAnnotations;

namespace VeterinaryPassport.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public List<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
        [Display(Name = "Владелец")]
        [Required(ErrorMessage = "Поле не указано")]
        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }
        [Display(Name = "Питомец")]
        [Required(ErrorMessage = "Поле не указано")]
        public int PetId { get; set; }
        public Pet? Pet { get; set; }
    }
}