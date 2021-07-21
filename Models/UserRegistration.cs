using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class UserRegistration : User
    {
        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [MinLength(8, ErrorMessage = "Пароль не может быть меньше 8 символов")]
        [DisplayName("Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать!")]
        public string PasswordConfirmation { get; set; }
    }
}