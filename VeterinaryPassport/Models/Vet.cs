﻿using System.ComponentModel.DataAnnotations;

namespace VeterinaryPassport.Models
{
    public class Vet
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        [RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Некорректный ввод")]
        [Required(ErrorMessage = "Поле не указано")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 30 символов")] // уменьшить длину
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
        [Display(Name = "Логин")]
        //[EmailAddress(ErrorMessage = "Почта указана не верна")]
        [Required(ErrorMessage = "Поле не указано")]
        public string Login { get; set; }
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле не указано")]
        public string Password { get; set; }
        public List<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
    }
}