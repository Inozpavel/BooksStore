using BooksStore.Models;
using BooksStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BooksStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreRepository _repository;

        public StoreController(IStoreRepository repository) => _repository = repository;

        [HttpGet]
        public ViewResult Index() => View(_repository.Books.Include(x => x.BookImages).ToList());

        [HttpGet]
        public FileContentResult GetImage(int imageId)
        {
            var image = _repository.FindImage(imageId);

            if (image != null)
                return File(image, "image/jpg");
            else
                return File(System.IO.File.ReadAllBytes("./wwwroot/images/NotFound.jpg"), "image/jpg");
        }


        [HttpGet]
        public ViewResult AllBooks() => View(_repository.Books);

        [HttpGet]
        public ViewResult AllAuthors() => View(_repository.Authors);

        [HttpGet]
        public ViewResult AllCategories() => View(_repository.Categories);

        [HttpGet]
        public IActionResult Book(int bookId)
        {
            Book book = _repository.FindBook(bookId);
            return book != null ? View(book) : NotFound();
        }

        [HttpGet]
        public IActionResult FindBooks(string searchOption) => View("Index", _repository.FindBooks(searchOption));

        #region Add

        [HttpGet]
        [Authorize(Policy = "CanChangeOrAddItems")]
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
        [Authorize(Policy = "CanChangeOrAddItems")]
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
        [Authorize]
        public IActionResult AddToCart(int bookId, string returnUrl = null, string query = null)
        {
            if (_repository.FindBook(bookId) == null)
                return NotFound();
            User user = _repository.FindUser(User.Identity.Name);
            CartItem item = _repository.FindCartItem(user.Id, bookId);
            if (item != null)
            {
                item.Count++;
                _repository.UpdateCartItem(item);
                ViewBag.CartMessage = "Еще один товар успешно добавлен в корзину!";
            }
            else
            {
                _repository.AddCartItem(new CartItem()
                {
                    BookId = bookId,
                    UserId = user.Id,
                    Count = 1
                });
                ViewBag.CartMessage = "Новый товар успешно добавлен в корзину!";
            }
            return Redirect(returnUrl + query);
        }

        [HttpGet]
        [Authorize(Policy = "CanChangeOrAddItems")]
        public ViewResult AddCategory() => ShowView("ChangeOrAddCategory", "Добавление жанра", new Category(), "Добавить");

        [HttpPost]
        [Authorize(Policy = "CanChangeOrAddItems")]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (_repository.FindCategory(category.Name) != null)
                {
                    ViewBag.ErrorMessage = "Жанр с таким названием уже добавлен!";
                    return ShowView("ChangeOrAddCategory", "Добавление жанра", category, "Добавить");
                }
                _repository.AddCategory(category);
                return RedirectToAction("AllCategories");
            }
            else
                return ShowView("ChangeOrAddCategory", "Добавление жанра", category, "Добавить");
        }

        [HttpGet]
        [Authorize(Policy = "CanChangeOrAddItems")]
        public ViewResult AddAuthor() => ShowView("ChangeOrAddAuthor", "Добавление автора", new Author(), "Добавить");

        [HttpPost]
        [Authorize(Policy = "CanChangeOrAddItems")]
        public IActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                if (_repository.FindAuthor(author.Name) != null)
                {
                    ViewBag.ErrorMessage = "Автор с таким именем уже добавлен!";
                    return ShowView("ChangeOrAddAuthor", "Добавление автора", author, "Добавить");
                }
                _repository.AddAuthor(author);
                return RedirectToActionPermanent("AllAuthors");
            }
            else
                return ShowView("ChangeOrAddAuthor", "Добавление автора", author, "Добавить");
        }

        #endregion

        #region Change

        [HttpGet]
        [Authorize(Policy = "CanChangeOrAddItems")]
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
        [Authorize(Policy = "CanChangeOrAddItems")]
        public IActionResult ChangeBook(BookInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = model.Book;
                book.BookImages = _repository.FindImages(book.Id) ?? new List<ProductImage>();

                List<byte[]> existingImages = book.BookImages.Select(x => x.Image).ToList();
                byte[] imageBytes;
                foreach (var image in model.UploadedImages ?? new List<IFormFile>())
                {
                    using var reader = new BinaryReader(image.OpenReadStream());
                    imageBytes = reader.ReadBytes((int)image.Length);
                    if (existingImages.Any(x => x.SequenceEqual(imageBytes)))
                        continue;
                    book.BookImages.Add(new ProductImage()
                    {
                        Image = imageBytes
                    });
                }

                book.Author = _repository.FindAuthor(book.Author.Name);
                book.Category = _repository.FindCategory(book.Category.Name);
                _repository.UpdateBook(book);
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
        [Authorize(Policy = "CanChangeOrAddItems")]
        public IActionResult ChangeCategory(int categoryId)
        {
            Category category = _repository.FindCategory(categoryId);
            if (category is null)
                return NotFound();
            return ShowView("ChangeOrAddCategory", "Изменение информации о жанре", category, "Сохранить");
        }

        [HttpPost]
        [Authorize(Policy = "CanChangeOrAddItems")]
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
        [Authorize(Policy = "CanChangeOrAddItems")]
        public IActionResult ChangeAuthor(int authorId)
        {
            Author author = _repository.FindAuthor(authorId);
            if (author is null)
                return NotFound();
            return ShowView("ChangeOrAddAuthor", "Изменение информации об авторе", author, "Сохранить");
        }

        [HttpPost]
        [Authorize(Policy = "CanChangeOrAddItems")]
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

        #region Remove

        [HttpGet]
        [Authorize]
        public IActionResult DecreaseCartItemCount(int bookId, string returnUrl = null, string query = null)
        {
            if (_repository.FindBook(bookId) == null)
                return NotFound();
            User user = _repository.FindUser(User.Identity.Name);
            CartItem item = _repository.FindCartItem(user.Id, bookId);
            if (item != null)
            {
                item.Count--;
                if (item.Count > 0)
                    _repository.UpdateCartItem(item);
                else
                    _repository.RemoveCartItem(item);
            }
            return Redirect(returnUrl + query);
        }

        [HttpGet]
        [Authorize]
        public IActionResult RemoveCartItem(int bookId, string returnUrl = null, string query = null)
        {
            if (_repository.FindBook(bookId) == null)
                return NotFound();
            User user = _repository.FindUser(User.Identity.Name);
            CartItem item = _repository.FindCartItem(user.Id, bookId);
            if (item != null)
                _repository.RemoveCartItem(item);

            return Redirect(returnUrl + query);
        }

        [HttpGet]
        [Authorize]
        public IActionResult RemoveAllItemsFromCart(string returnUrl = null, string query = null)
        {
            User user = _repository.FindUser(User.Identity.Name);
            _repository.RemoveCartItems(user.Id);
            return Redirect(returnUrl + query);
        }

        [HttpGet]
        [Authorize(Policy = "CanRemoveItems")]
        public IActionResult RemoveBook(int bookId) => RemoveElement("Товар", _repository.Remove(_repository.FindBook(bookId)), "AllBooks");

        [HttpGet]
        [Authorize(Policy = "CanRemoveItems")]
        public IActionResult RemoveCategory(int categoryId) => RemoveElement("Жанр", _repository.Remove(_repository.FindCategory(categoryId)), "AllCategories");

        [HttpGet]
        [Authorize(Policy = "CanRemoveItems")]
        public IActionResult RemoveAuthor(int authorId) => RemoveElement("Автор", _repository.Remove(_repository.FindAuthor(authorId)), "AllAuthors");

        private IActionResult RemoveElement(string itemName, INameable nameable, string redirectToActionName)
        {
            TempData["DeletedElementMessage"] = $"{itemName} \"{nameable.Name}\" был успешно удален.";
            return RedirectToActionPermanent(redirectToActionName);
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
