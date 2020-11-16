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

        Book RemoveBook(Book book);

        Category RemoveCategory(Category category);

        Author RemoveAuthor(Author author);

        INameable Remove<T>(T element) where T : INameable;

        void UpdateBook(Book book);

        void UpdateCategory(Category category);

        void UpdateAuthor(Author author);

        void SaveChanges();
    }
}
