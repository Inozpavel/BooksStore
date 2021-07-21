using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Category : INameable
    {
        public Category() : this("", null)
        {
        }

        public Category(string name) => Name = name;

        public Category(string name, string description) : this(name) => Description = description;

        public int Id { get; set; }

        [DisplayName("Жанр")]
        [Required(ErrorMessage = "У жанра должно быть название!")]
        public string Name { get; set; }

        [DisplayName("Описание жанра")]
        public string Description { get; set; }

        public override string ToString() => Name;
    }
}