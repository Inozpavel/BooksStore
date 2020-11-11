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
    }
}
