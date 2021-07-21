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

        public IQueryable<CartItem> CartItems { get; }

        bool CheckEmailAlreadyExists(string email);

        #region Find

        User FindUser(int userId);

        User FindUser(string email, string password);

        User FindUser(string email);

        Book FindBook(int bookId);

        IEnumerable<Book> FindBooks(string searchOption);

        Author FindAuthor(int authorId);

        Author FindAuthor(string authorName);

        Category FindCategory(int categoryId);

        Category FindCategory(string categoryName);

        byte[] FindImage(int imageId);

        List<ProductImage> FindImages(int bookId);

        Role FindRole(string roleName);

        CartItem FindCartItem(int userId, int bookId);

        IEnumerable<CartItem> FindCartItems(int userId);

        #endregion

        #region Add

        void AddBook(Book book);

        void AddBooksRange(params Book[] books);

        void AddAuthor(Author author);

        void AddAuthorsRange(params Author[] authors);

        void AddCategory(Category category);

        void AddCategoriesRange(params Category[] categories);

        void AddUser(User user);

        void AddRole(Role role);

        void AddCartItem(CartItem cartItem);

        #endregion

        #region Remove

        INameable RemoveBook(Book book);

        INameable RemoveCategory(Category category);

        INameable RemoveAuthor(Author author);

        void RemoveCartItem(CartItem cartItem);

        void RemoveCartItems(int userId);

        INameable Remove<T>(T element) where T : INameable;

        #endregion

        #region Update

        void UpdateBook(Book book);

        void UpdateCategory(Category category);

        void UpdateAuthor(Author author);

        void UpdateUser(User user);

        void UpdateCartItem(CartItem cartItem);

        #endregion
    }
}