using System.Collections.Generic;
using System.Linq;
using BooksStore.Controllers;
using BooksStore.Models;
using BooksStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BooksStore.Tests
{
    public class StoreControllerTests
    {
        [Fact]
        public void CanLoadBooks()
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new()
                {
                    Name = "Сказка о рыбаке и рыбке",
                    Category = new Category("Сказки"),
                    Author = new Author("Пушкин А. С.")
                },
                new()
                {
                    Name = "Война и мир",
                    Category = new Category("Романы"),
                    Author = new Author("Толстой Л. Н.")
                },
                new()
                {
                    Name = "Преступление и наказание",
                    Category = new Category("Романы"),
                    Author = new Author("Достоевский Ф. М.")
                }
            }.AsQueryable());
            var controller = new StoreController(mock.Object);
            var result = (controller.AllBooks().Model as IEnumerable<Book>)!.ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("Преступление и наказание", result[2].Name);
            Assert.Equal("Романы", result[2].Category.Name);
            Assert.Equal("Достоевский Ф. М.", result[2].Author.Name);
        }

        [Fact]
        public void CanLoadCategories()
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Categories)
                .Returns(new List<Category> {new("Сказки"), new("Романы"), new("Повести")}.AsQueryable());
            var controller = new StoreController(mock.Object);
            var result = (controller.AllCategories().Model as IEnumerable<Category>)!.ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("Сказки", result[0].Name);
            Assert.Equal("Романы", result[1].Name);

            Assert.Equal("Повести", result[2].Name);
        }

        [Fact]
        public void CanLoadAuthors()
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Authors).Returns(new List<Author>
            {
                new("Пушкин А. С."), new("Толстой Л. Н."), new("Достоевский Ф. М.")
            }.AsQueryable());
            var controller = new StoreController(mock.Object);
            var result = (controller.AllAuthors().Model as IEnumerable<Author>)!.ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal("Пушкин А. С.", result[0].Name);
            Assert.Equal("Толстой Л. Н.", result[1].Name);
            Assert.Equal("Достоевский Ф. М.", result[2].Name);
        }

        [Fact]
        public void AddBookWillRedirectToAllBooks()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            var book = new Book();
            var bookInputView = new BookInputViewModel {Book = book};

            string actualName = (controller.AddBook(bookInputView) as RedirectToActionResult)?.ActionName;
            Assert.Equal("AllBooks", actualName);
            mock.Verify(x => x.AddBook(bookInputView.Book));
        }

        [Fact]
        public void AddAuthorWillRedirectToAllAuthors()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            var author = new Author();

            string actualName = (controller.AddAuthor(author) as RedirectToActionResult)?.ActionName;
            Assert.Equal("AllAuthors", actualName);
            mock.Verify(x => x.AddAuthor(author));
        }

        [Fact]
        public void AddCategoryWillRedirectToAllCategories()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            var category = new Category();

            string actualName = (controller.AddCategory(category) as RedirectToActionResult)?.ActionName;
            Assert.Equal("AllCategories", actualName);
            mock.Verify(x => x.AddCategory(category));
        }

        [Theory]
        [InlineData("Пушкин А. С.", "Автор с таким именем уже добавлен!")]
        [InlineData("Толстой Л. Н.", "Автор с таким именем уже добавлен!")]
        [InlineData("Достоевский Ф. М.", "Автор с таким именем уже добавлен!")]
        [InlineData("Лесков Н. С.", null)]
        [InlineData("Салтыков-Шедрин М. Е.", null)]
        public void AddAuthorWillShowMessageAuthorAlreadyAdded(string authorName, string errorMessage)
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Authors).Returns(new List<Author>
            {
                new("Пушкин А. С."), new("Толстой Л. Н."), new("Достоевский Ф. М.")
            }.AsQueryable());
            mock.Setup(x => x.FindAuthor(authorName))
                .Returns(mock.Object.Authors.FirstOrDefault(x => x.Name == authorName));

            StoreController controller = new(mock.Object);

            controller.AddAuthor(new Author(authorName));
            Assert.Equal(errorMessage, controller.ViewBag.ErrorMessage);
        }

        [Theory]
        [InlineData("Сказки", "Жанр с таким названием уже добавлен!")]
        [InlineData("Басни", "Жанр с таким названием уже добавлен!")]
        [InlineData("Рассказы", "Жанр с таким названием уже добавлен!")]
        [InlineData("Комедии", null)]
        [InlineData("Трагедия", null)]
        public void AddCategoryWillShowMessageCategoryAlreadyAdded(string categoryName, string errorMessage)
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Categories)
                .Returns(new List<Category> {new("Сказки"), new("Басни"), new("Рассказы")}.AsQueryable());
            mock.Setup(x => x.FindCategory(categoryName))
                .Returns(mock.Object.Categories.FirstOrDefault(x => x.Name == categoryName));

            StoreController controller = new(mock.Object);

            controller.AddCategory(new Category(categoryName));
            Assert.Equal(errorMessage, controller.ViewBag.ErrorMessage);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public void ChangeAuthorWillReturnNotFoundWhenIdNotFound(int id, bool idExists)
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Authors).Returns(new List<Author>
            {
                new("Пушкин А. С.") {Id = 1}, new("Толстой Л. Н.") {Id = 2}, new("Достоевский Ф. М.") {Id = 3}
            }.AsQueryable());
            mock.Setup(x => x.FindAuthor(id)).Returns(mock.Object.Authors.FirstOrDefault(x => x.Id == id));
            var controller = new StoreController(mock.Object);
            var view = controller.ChangeAuthor(id);

            if (idExists)
                Assert.IsNotType<NotFoundResult>(view);
            else
                Assert.IsType<NotFoundResult>(view);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public void ChangeCategoryWillReturnNotFoundWhenIdNotFound(int id, bool idExists)
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Categories).Returns(new List<Category>
            {
                new("Комедии") {Id = 1}, new("Повести") {Id = 2}, new("Рассказы") {Id = 3}
            }.AsQueryable());
            mock.Setup(x => x.FindCategory(id)).Returns(mock.Object.Categories.FirstOrDefault(x => x.Id == id));
            var controller = new StoreController(mock.Object);
            var view = controller.ChangeCategory(id);

            if (idExists)
                Assert.IsNotType<NotFoundResult>(view);
            else
                Assert.IsType<NotFoundResult>(view);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public void ChangeBookWillReturnNotFoundWhenIdNotFound(int id, bool idExists)
        {
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new() {Id = 1, Category = new Category("Комедии"), Author = new Author("Салтыков-Шедрин М. Е.")},
                new() {Id = 2, Category = new Category("Повести"), Author = new Author("Лесков Н. С.")},
                new() {Id = 3, Category = new Category("Рассказы"), Author = new Author("Толстой Л. Н.")}
            }.AsQueryable());
            mock.Setup(x => x.FindBook(id)).Returns(mock.Object.Books.FirstOrDefault(x => x.Id == id));
            var controller = new StoreController(mock.Object);
            var view = controller.ChangeBook(id);

            if (idExists)
                Assert.IsNotType<NotFoundResult>(view);
            else
                Assert.IsType<NotFoundResult>(view);
        }

        [Fact]
        public void ChangeBookReturnsViewResultWithUserModel()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            controller.ModelState.AddModelError("Name", "Название книги не может быть пустым!");
            var book = new Book();
            var model = new BookInputViewModel
            {
                Categories = new List<Category>(), Authors = new List<Author>(), Book = book
            };
            var result = controller.ChangeBook(model) as ViewResult;
            Assert.IsType<ViewResult>(result);
            Assert.Equal(book, (result.Model as BookInputViewModel)?.Book);
        }

        [Fact]
        public void ChangeCategoryReturnsViewResultWithUserModel()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            var category = new Category();
            controller.ModelState.AddModelError("Name", "Название не может быть пустым!");

            var result = controller.ChangeCategory(category) as ViewResult;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(category, result.Model);
        }

        [Fact]
        public void AddAuthorReturnsViewResultWithUserModel()
        {
            var mock = new Mock<IStoreRepository>();
            var controller = new StoreController(mock.Object);
            var author = new Author();
            controller.ModelState.AddModelError("Name", "Имя не может быть пустым!");

            var result = controller.ChangeAuthor(author) as ViewResult;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(author, result.Model);
        }
    }
}