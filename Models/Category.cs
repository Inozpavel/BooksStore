using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Category
    {
        [Key]
        [DisplayName("Категория")]
        [Required(ErrorMessage ="У категории должно быть название!")]
        public string Name { get; set; }

        [DisplayName("Описание категории")]
        public string Description { get; set; }

        public Category() : this("", null)
        {
        }

        public Category(string name) => Name = name;

        public Category(string name, string description) : this(name) => Description = description;

        public override string ToString() => Name;
    }
}
