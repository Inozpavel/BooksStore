using System.Collections.Generic;
using BooksStore.Models;
using Microsoft.AspNetCore.Http;

namespace BooksStore.ViewModels
{
    public class BookInputViewModel
    {
        public Book Book { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        public IEnumerable<IFormFile> UploadedImages { get; set; }
    }
}