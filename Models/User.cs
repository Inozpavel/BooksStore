using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class User
    {
        public int Id { get; set; }

        public DateTime RegistrationTime = DateTime.Now;

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [DisplayName("Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [DisplayName("Фамилия")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [EmailAddress(ErrorMessage = "Значение долно быть в формате адреса почты!")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [DisplayName("Номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [MinLength(8, ErrorMessage = "Пароль не может быть меньше 8 символов")]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}
