using BooksStore.Models;
using BooksStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreRepository _repository;

        public StoreController(IStoreRepository repository) => _repository = repository;

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
        public ViewResult AllBooks() => View(_repository.BooksWithAllFields);

        [HttpGet]
        public ViewResult AllAuthors() => View(_repository.Authors);

        [HttpGet]
        public ViewResult AllCategories() => View(_repository.Categories);

        #region Add

        [HttpGet]
        public ViewResult AddBook()
        {
            BookInputViewModel model = new BookInputViewModel()
            {
                Authors = _repository.AuthorsOrderedByName,
                Categories = _repository.CategoriesOrderedByName,
                Book = new Book()
                {
                    Author = new Author(),
                    Category = new Category()
                }
            };
            return ShowView("ChangeOrAddBook", "Добавление книги", model, "Добавить");
        }

        [HttpPost]
        public IActionResult AddBook(BookInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.AddBook(model.Book);
                return RedirectToAction("AllBooks");
            }
            else
            {
                model.Authors = _repository.AuthorsOrderedByName;
                model.Categories = _repository.CategoriesOrderedByName;
                return ShowView("ChangeOrAddBook", "Добавление книги", model, "Добавить");
            }
        }

        [HttpGet]
        public ViewResult AddCategory() => ShowView("ChangeOrAddCategory", "Добавление жанра", new Category(), "Добавить");

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (_repository.FindCategory(category.Name) != null)
                {
                    TempData["ErrorMessage"] = "Жанр с таким названием уже добавлен!";
                    return ShowView("ChangeOrAddCategory", "Добавление жанра", category, "Добавить");
                }
                _repository.AddCategory(category);
                return RedirectToAction("AllCategories");
            }
            else
                return ShowView("ChangeOrAddCategory", "Добавление жанра", category, "Добавить");
        }

        [HttpGet]
        public ViewResult AddAuthor() => ShowView("ChangeOrAddAuthor", "Добавление автора", new Author(), "Добавить");

        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                if (_repository.FindAuthor(author.Name) != null)
                {
                    TempData["ErrorMessage"] = "Автор с таким именем уже добавлен!";
                    return ShowView("ChangeOrAddAuthor", "Добавление автора", author, "Добавить");
                }
                _repository.AddAuthor(author);
                return RedirectToAction("AllAuthors");
            }
            else
                return ShowView("ChangeOrAddAuthor", "Добавление автора", author, "Добавить");
        }

        #endregion

        #region Change

        [HttpGet]
        public IActionResult ChangeBook(int bookId)
        {
            Book book = _repository.FindBook(bookId);
            if (book is null)
                return NotFound();
            BookInputViewModel model = new BookInputViewModel()
            {
                Authors = _repository.AuthorsOrderedByName,
                Categories = _repository.CategoriesOrderedByName,
                Book = book
            };
            return ShowView("ChangeOrAddBook", "Изменение информации о книге", model, "Сохранить");
        }

        [HttpPost]
        public IActionResult ChangeBook(BookInputViewModel model)
        {
            model.Book.Author = _repository.FindAuthor(model.Book.Author.Name);
            model.Book.Category = _repository.FindCategory(model.Book.Category.Name);
            if (ModelState.IsValid)
            {
                _repository.UpdateBook(model.Book);
                return RedirectToAction("AllBooks");
            }
            else
            {
                BookInputViewModel viewModel = new BookInputViewModel()
                {
                    Authors = _repository.AuthorsOrderedByName,
                    Categories = _repository.CategoriesOrderedByName,
                    Book = model.Book
                };
                return ShowView("ChangeOrAddBook", "Изменение информации о книге", viewModel, "Сохранить");
            }
        }

        [HttpGet]
        public IActionResult ChangeCategory(int categoryId)
        {
            Category category = _repository.FindCategory(categoryId);
            if (category is null)
                return NotFound();
            return ShowView("ChangeOrAddCategory", "Изменение информации о жанре", category, "Сохранить");
        }

        [HttpPost]
        public IActionResult ChangeCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateCategory(category);
                return RedirectToAction("AllCategories");
            }
            else
                return View("ChangeOrAddCategory", category);
        }

        [HttpGet]
        public IActionResult ChangeAuthor(int authorId)
        {
            Author author = _repository.FindAuthor(authorId);
            if (author is null)
                return NotFound();
            return ShowView("ChangeOrAddAuthor", "Изменение информации об авторе", author, "Сохранить");
        }

        [HttpPost]
        public IActionResult ChangeAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateAuthor(author);
                return RedirectToAction("AllAuthors");
            }
            else
                return View("ChangeOrAddAuthor", author);
        }

        #endregion

        #region Delete

        [HttpGet]
        public IActionResult DeleteBook(int bookId)
        {
            Book book = _repository.RemoveBook(bookId);
            TempData["deletedBookMessage"] = $"Книга \"{book.Name}\" была успешно удалена";
            return RedirectToAction("AllBooks");
        }

        [HttpGet]
        public IActionResult DeleteCategory(int categoryId)
        {
            Category category = _repository.RemoveCategory(categoryId);
            TempData["deletedCategoryMessage"] = $"Жанр \"{category.Name}\" был успешно удален";
            return RedirectToAction("AllCategories");
        }

        [HttpGet]
        public IActionResult DeleteAuthor(int authorId)
        {
            Author author = _repository.RemoveAuthor(authorId);
            TempData["deletedAuthorMessage"] = $"Автор \"{author.Name}\" был успешно удалён";
            return RedirectToAction("AllAuthors");
        }

        #endregion

        private ViewResult ShowView(string viewName, string viewTitle, object model, string buttonText)
        {
            ViewBag.Title = viewTitle;
            ViewBag.ButtonText = buttonText;
            return View(viewName, model);
        }
    }
}
