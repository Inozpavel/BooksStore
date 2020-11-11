using BooksStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BooksStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreContext _context;

        public StoreController(StoreContext context) => _context = context;

        [HttpGet]
        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult Login() => View();

        [HttpGet]
        public ViewResult Registration() => View();

        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (ModelState.IsValid == false)
                return View();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult AllBooks()
        {
            var t = _context.Books.ToArray();
            return View(_context.Books.Include(x => x.Author).Include(x => x.Category).ToList());
        }

        [HttpGet]
        public ViewResult ChangeBook(int bookId)
        {
            ViewBag.Title = "Изменение информации о книге";
            ViewBag.MainButtonText = "Сохранить";
            Book book = _context.Books.Include(x => x.Author).Include(x => x.Category).FirstOrDefault(x => x.Id == bookId);
            return View("ChangeOrAddBook", book);
        }

        [HttpPost]
        public IActionResult ChangeBook(Book book)
        {
            if (ModelState.IsValid)
            {
                if (_context.Categories.Contains(book.Category) == false)
                {
                    _context.Categories.Add(book.Category);
                    _context.SaveChanges();
                }

                if (_context.Authors.Contains(book.Author) == false)
                {
                    _context.Authors.Add(book.Author);
                    _context.SaveChanges();
                }

                _context.Books.Update(book);
                _context.SaveChanges();
                return RedirectToAction("AllBooks");
            }
            else
                return View("ChangeOrAddBook", book);

        }

        [HttpGet]
        public IActionResult DeleteBook(int bookId)
        {
            Book book = _context.Books.FirstOrDefault(book => book.Id == bookId);
            _context.Books.Remove(book);
            _context.SaveChanges();
            TempData["deletedBookMessage"] = $"{book.Name} был(-а) удалена";
            return RedirectToAction("AllBooks");
        }

        [HttpGet]
        public ViewResult AddBook()
        {
            ViewBag.Title = "Добавление книги";
            ViewBag.MainButtonText = "Добавить";
            return View("ChangeOrAddBook");
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Book()
                {
                    Name = book.Name,
                    Author = new Author(book.Author.Name),
                    Category = new Category(book.Category.Name),
                    Description = book.Description
                });
                _context.SaveChanges();
                return RedirectToAction("AllBooks");
            }
            else
                return View("ChangeOrAddBook", book);
        }
    }
}
