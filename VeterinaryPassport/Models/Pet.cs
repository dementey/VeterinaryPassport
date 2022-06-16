using System.ComponentModel.DataAnnotations;

namespace VeterinaryPassport.Models
{
    public class Pet
    {
        public int Id { get; set; }
        [Display(Name = "Кличка")]
        //[RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 2 до 30 символов")]
        public string Name { get; set; }
        [Display(Name = "Вид")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 2 до 30 символов")]
        public string Kind { get; set; }
        [Display(Name = "Пол")]
        [Required(ErrorMessage = "Поле не указано")]
        public string Sex { get; set; }
        [Display(Name = "Порода")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 2 до 30 символов")]
        public string Breed { get; set; }
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Поле не указано")]
        public DateTime DOB { get; set; }
        public Passport? Passport { get; set; }
    }
}
