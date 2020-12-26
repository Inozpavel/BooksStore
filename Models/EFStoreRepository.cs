using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreContext _context;

        public IQueryable<Book> Books => _context.Books.Include(x => x.Author).Include(x => x.Category);

        public IQueryable<Author> Authors => _context.Authors;

        public IQueryable<Category> Categories => _context.Categories;

        public IEnumerable<Author> AuthorsOrderedByName => _context.Authors.OrderBy(author => author.Name).ToList();

        public IEnumerable<Category> CategoriesOrderedByName => _context.Categories.OrderBy(category => category.Name).ToList();

        public IQueryable<User> Users => _context.Users;

        public IQueryable<Role> Roles => _context.Roles;

        public IQueryable<ProductImage> Images => _context.Images;

        public IQueryable<CartItem> CartItems => _context.CartsItems;

        public EFStoreRepository(StoreContext context) => _context = context;

        #region Find

        public User FindUser(int userId) => Users.Include(x => x.Role).FirstOrDefault(user => user.Id == userId);

        public User FindUser(string email, string password) => Users.Include(x => x.Role).FirstOrDefault(user => user.Email == email && user.Password == password);

        public User FindUser(string email) => Users.Include(x => x.Role).Include(x => x.CartsItems).FirstOrDefault(user => user.Email == email);

        public Book FindBook(int bookId) => Books.Include(x => x.BookImages).FirstOrDefault(book => book.Id == bookId);

        public IEnumerable<Book> FindBooks(string searchOption) => Books.Include(x => x.BookImages).Where(book => book.Name.Contains(searchOption) || book.Author.Name.Contains(searchOption) || book.Category.Name.Contains(searchOption)).ToList();

        public Author FindAuthor(int authorId) => Authors.FirstOrDefault(author => author.Id == authorId);

        public Author FindAuthor(string authorName) => Authors.FirstOrDefault(author => author.Name == authorName);

        public Category FindCategory(int categoryId) => Categories.FirstOrDefault(category => category.Id == categoryId);

        public Category FindCategory(string categoryName) => Categories.FirstOrDefault(category => category.Name == categoryName);

        public Role FindRole(string roleName) => Roles.FirstOrDefault(role => role.Name == roleName);

        public byte[] FindImage(int imageId) => Images.FirstOrDefault(image => image.Id == imageId)?.Image;

        public List<ProductImage> FindImages(int bookId) => Images.Where(x => x.BookId == bookId).ToList();

        public CartItem FindCartItem(int userId, int bookId) => CartItems.FirstOrDefault(item => item.UserId == userId && item.BookId == bookId);

        public IEnumerable<CartItem> FindCartItems(int userId) => CartItems.Include(x => x.Book).Where(item => item.UserId == userId).ToList();

        #endregion

        #region Add

        public void AddBook(Book book)
        {
            book.Author = FindAuthor(book.Author.Name) ?? book.Author;
            book.Category = FindCategory(book.Category.Name) ?? book.Category;
            _context.Add(book);
            _context.SaveChanges();
        }

        public void AddBooksRange(params Book[] books) => books.ToList().ForEach(book => AddBook(book));

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void AddAuthorsRange(params Author[] authors) => authors.ToList().ForEach(author => AddAuthor(author));

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void AddCategoriesRange(params Category[] categories) => categories.ToList().ForEach(category => AddCategory(category));

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void AddCartItem(CartItem cartItem)
        {
            _context.CartsItems.Add(cartItem);
            _context.SaveChanges();
        }

        #endregion

        #region Update

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateCartItem(CartItem cartItem)
        {
            _context.CartsItems.Update(cartItem);
            _context.SaveChanges();
        }

        #endregion

        #region Remove

        public INameable RemoveBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }

        public INameable RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }

        public INameable RemoveAuthor(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return author;
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartsItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void RemoveCartItems(int userId)
        {
            List<CartItem> items = _context.CartsItems.ToList();
            foreach (var item in items)
                _context.CartsItems.Remove(item);
            _context.SaveChanges();
        }

        public INameable Remove<T>(T element) where T : INameable
        {
            return element switch
            {
                Book book => RemoveBook(book),
                Author author => RemoveAuthor(author),
                Category category => RemoveCategory(category),
                _ => throw new ArgumentException("Невозможной удалить выбранный тип!")
            };
        }

        #endregion

        public bool CheckEmailAlreadyExists(string email) => _context.Users.Any(x => x.Email == email);
    }
}
