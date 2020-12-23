using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Models
{
    public interface IStoreRepository
    {
        public IQueryable<Book> Books { get; }

        public IQueryable<Category> Categories { get; }

        public IQueryable<Author> Authors { get; }

        public IEnumerable<Author> AuthorsOrderedByName { get; }

        public IEnumerable<Category> CategoriesOrderedByName { get; }

        public IQueryable<Role> Roles { get; }

        public IQueryable<User> Users { get; }

        public IQueryable<ProductImage> Images { get; }

        #region Find

        User FindUser(int userId);

        User FindUser(string email, string password);

        User FindUser(string email);

        Book FindBook(int bookId);

        Author FindAuthor(int authorId);

        Author FindAuthor(string authorName);

        Category FindCategory(int categoryId);

        Category FindCategory(string categoryName);

        byte[] FindImage(int imageId);

        List<ProductImage> FindImages(int bookId);

        Role FindRole(string roleName);

        #endregion

        #region Add

        void AddBook(Book book);

        void AddAuthor(Author author);

        void AddCategory(Category category);

        void AddBooksRange(params Book[] books);

        void AddUser(User user);

        void AddRole(Role role);

        #endregion

        #region Remove

        INameable RemoveBook(Book book);

        INameable RemoveCategory(Category category);

        INameable RemoveAuthor(Author author);

        INameable Remove<T>(T element) where T : INameable;

        #endregion

        #region Update

        void UpdateBook(Book book);

        void UpdateCategory(Category category);

        void UpdateAuthor(Author author);

        void UpdateUser(User user);

        #endregion

        bool CheckEmailAlreadyExists(string email);
    }
}
