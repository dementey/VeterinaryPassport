using System.ComponentModel.DataAnnotations;

namespace VeterinaryPassport.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 30 символов")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 30 символов")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 30 символов")]
        public string Patronymic { get; set; }
        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Поле не указано")]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Длина должна быть 12 цифр")]
        public string PhoneNumber { get; set; }
        public List<Passport> Passports { get; set; } = new List<Passport>();
    }
}
