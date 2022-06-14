using System.ComponentModel.DataAnnotations;

namespace VeterinaryPassport.Models
{
    public class Vaccine
    {
        public int Id { get; set; }
        [Display(Name = "Название прививки")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 30 символов")]
        public string Name { get; set; }
        [Display(Name = "Дата получения")]
        [Required(ErrorMessage = "Поле не указано")]
        public DateTime DateVaccination { get; set; }
        public int PassportId { get; set; }
        public Passport Passport { get; set; }
        [Display(Name = "Ветеринар")]
        public int VetId { get; set; }
        public Vet Vet { get; set; }
    }
}
