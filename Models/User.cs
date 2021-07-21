using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class User : UserLogin
    {
        public int Id { get; set; }

        public DateTime RegistrationTime { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [DisplayName("Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Значение не может быть пустым!")]
        [DisplayName("Фамилия")]
        public string SecondName { get; set; }

        [DisplayName("Контактный телефон")]
        public string Phone { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public List<CartItem> CartsItems { get; set; }
    }
}