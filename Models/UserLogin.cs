using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStore.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [EmailAddress(ErrorMessage = "Значение долно быть в формате адреса почты!")]
        [DisplayName("Почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [MinLength(8, ErrorMessage = "Пароль не может быть меньше 8 символов")]
        [DisplayName("Пароль")]
        [Compare("PasswordConfirmation", ErrorMessage = "Пароли должны совпадать!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [MinLength(8, ErrorMessage = "Пароль не может быть меньше 8 символов")]
        [DisplayName("Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать!")]
        [NotMapped]
        public string PasswordConfirmation { get; set; }

        [NotMapped]
        public bool ShouldRememberUser { get; set; }
    }
}
