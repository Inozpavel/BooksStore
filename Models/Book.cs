using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Book : INameable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "У книги должно быть название!")]
        [DisplayName("Название книги")]
        public string Name { get; set; }

        [DisplayName("Цена товара")]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int ProductImageId { get; set; }

        public List<ProductImage> BookImages { get; set; }

        [DisplayName("Описание книги")]
        public string Description { get; set; }

    }
}
