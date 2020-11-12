using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Models
{
    public interface IStoreRepository
    {
        public DbSet<Book> Books { get; }

        public DbSet<Category> Categories { get; }

        public DbSet<Author> Authors { get; }

        public IEnumerable<Book> BooksWithAllFields { get; }

        public IEnumerable<Author> AuthorsOrderedByName { get; }

        public IEnumerable<Category> CategoriesOrderedByName { get; }

        Book FindBook(int bookId);

        Author FindAuthor(int authorId);

        Author FindAuthor(string authorName);

        Category FindCategory(int categoryId);

        Category FindCategory(string categoryName);

        void AddBook(Book book);

        void AddAuthor(Author author);

        void AddAuthor(string authorName);

        void AddCategory(Category category);

        void AddCategory(string categoryName);

        void AddBooksRange(params Book[] books);

        Book RemoveBook(int bookId);

        Category RemoveCategory(int categoryId);

        Author RemoveAuthor(int authorId);

        void UpdateBook(Book book);

        void UpdateCategory(Category category);

        void UpdateAuthor(Author author);

        void SaveChanges();
    }
}
