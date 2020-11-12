using BooksStore.Models;
using System.Collections.Generic;

namespace BooksStore.ViewModels
{
    public class BookInputViewModel
    {
        public Book Book { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Author> Authors { get; set; }
    }
}
