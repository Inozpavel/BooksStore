using BooksStore.Models;
using Microsoft.AspNetCore.Mvc;

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

    }
}
