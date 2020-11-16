using System.Collections.Generic;

namespace BooksStore.Models
{
    public interface IStoreRepository
    {
        public IEnumerable<Book> Books { get; }

        public IEnumerable<Category> Categories { get; }

        public IEnumerable<Author> Authors { get; }

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
