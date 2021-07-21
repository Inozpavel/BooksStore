using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Author : INameable
    {
        public Author() : this("", null)
        {
        }

        public Author(string name) => Name = name;

        public Author(string name, string description) : this(name) => Description = description;

        public int Id { get; set; }

        [DisplayName("Имя автора")]
        [Required(ErrorMessage = "У автора должно быть имя!")]
        public string Name { get; set; }

        [DisplayName("Описание автора")]
        public string Description { get; set; }

        public override string ToString() => Name;
    }
}