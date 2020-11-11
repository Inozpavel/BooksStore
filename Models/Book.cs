using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="У книги должно быть название!")]
        [DisplayName("Название книги")]
        public string Name { get; set; }
        
        public Author Author { get; set; }
        
        public Category Category { get; set; }

        [DisplayName("Описание книги")]
        public string Description { get; set; }

        public Book()
        {

        }
    }
}
