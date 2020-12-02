using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreContext _context;

        public IEnumerable<Book> Books => _context.Books.Include(x => x.Author).Include(x => x.Category).ToList();

        public IEnumerable<Author> Authors => _context.Authors.ToList();

        public IEnumerable<Category> Categories => _context.Categories.ToList();

        public IEnumerable<Author> AuthorsOrderedByName => _context.Authors.OrderBy(author => author.Name).ToList();

        public IEnumerable<Category> CategoriesOrderedByName => _context.Categories.OrderBy(category => category.Name).ToList();

        public IEnumerable<User> Users => _context.Users.ToList();

        public IEnumerable<Role> Roles => _context.Roles.ToList();

        public EFStoreRepository(StoreContext context) => _context = context;

        #region Find

        public User FindUser(int userId) => _context.Users.FirstOrDefault(user => user.Id == userId);

        public User FindUser(string email, string password) => _context.Users.FirstOrDefault(user => user.Email == email && user.Password == password);

        public Book FindBook(int bookId) => _context.Books.FirstOrDefault(book => book.Id == bookId);

        public Author FindAuthor(int authorId) => _context.Authors.FirstOrDefault(author => author.Id == authorId);

        public Author FindAuthor(string authorName) => _context.Authors.FirstOrDefault(author => author.Name == authorName);

        public Category FindCategory(int categoryId) => _context.Categories.FirstOrDefault(category => category.Id == categoryId);

        public Category FindCategory(string categoryName) => _context.Categories.FirstOrDefault(category => category.Name == categoryName);

        public Role FindRole(string roleName) => _context.Roles.FirstOrDefault(role => role.Name == roleName);

        #endregion

        #region Add

        public void AddBook(Book book)
        {
            _context.Add(new Book()
            {
                Name = book.Name,
                Author = FindAuthor(book.Author.Name) ?? book.Author,
                Category = FindCategory(book.Category.Name) ?? book.Category,
                Description = book.Description
            });
            _context.SaveChanges();
        }

        public void AddBooksRange(params Book[] books)
        {
            foreach (var book in books)
            {
                _context.Books.Add(new Book()
                {
                    Name = book.Name,
                    Author = FindAuthor(book.Author.Name) ?? book.Author,
                    Category = FindCategory(book.Category.Name) ?? book.Category,
                    Description = book.Description
                });
                _context.SaveChanges();
            }
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(new Author()
            {
                Name = author.Name,
                Description = author.Description
            });
            _context.SaveChanges();

        }

        public void AddAuthor(string authorName)
        {
            _context.Authors.Add(new Author()
            {
                Name = authorName
            });
            _context.SaveChanges();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(new Category()
            {
                Name = category.Name,
                Description = category.Description
            });
            _context.SaveChanges();
        }

        public void AddCategory(string categoryName)
        {
            _context.Categories.Add(new Category()
            {
                Name = categoryName
            });
            _context.SaveChanges();
        }

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

        #endregion

        #region Remove

        public Book RemoveBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }

        public Category RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }

        public Author RemoveAuthor(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return author;
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
        public void SaveChanges() => _context.SaveChanges();

        #endregion
        public bool CheckEmailAlreadyExists(string email) => _context.Users.Any(x => x.Email == email);

        public bool CheckPhoneAlreadyExists(string phone) => _context.Users.Any(x => x.Phone == phone);
    }
}
