using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BooksStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private readonly StoreContext _context;

        public DbSet<Book> Books => _context.Books;

        public DbSet<Author> Authors => _context.Authors;

        public DbSet<Category> Categories => _context.Categories;

        public IEnumerable<Book> BooksWithAllFields => _context.Books.Include(x => x.Author).Include(x => x.Category);

        public IEnumerable<Author> AuthorsOrderedByName => _context.Authors.OrderBy(author => author.Name).ToList();

        public IEnumerable<Category> CategoriesOrderedByName => _context.Categories.OrderBy(category => category.Name).ToList();

        public EFStoreRepository(StoreContext context) => _context = context;

        #region Find

        public Book FindBook(int bookId) => _context.Books.Find(bookId);

        public Author? FindAuthor(int authorId) => _context.Authors.Find(authorId);

        public Author? FindAuthor(string authorName) => _context.Authors.FirstOrDefault(author => author.Name == authorName);

        public Category? FindCategory(int categoryId) => _context.Categories.Find(categoryId);

        public Category? FindCategory(string categoryName) => _context.Categories.FirstOrDefault(category => category.Name == categoryName);

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

        public Book RemoveBook(int bookId)
        {
            Book book = _context.Books.Find(bookId);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }

        public Category RemoveCategory(int categoryId)
        {
            Category category = _context.Categories.Find(categoryId);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }

        public Author RemoveAuthor(int authorId)
        {
            Author author = _context.Authors.Find(authorId);
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return author;
        }

        public void SaveChanges() => _context.SaveChanges();

        #endregion
    }
}
