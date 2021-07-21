using System.Collections.Generic;
using System.Linq;
using BooksStore.Controllers;
using BooksStore.Models;
using Moq;
using Xunit;

namespace BooksStore.Tests
{
    public class AccountControllerTests
    {
        [Theory]
        [InlineData("one@mail.ru", true)]
        [InlineData("two@mail.ru", false)]
        [InlineData("three@mail.ru", true)]
        [InlineData("ten@mail.ru", false)]
        [InlineData("twelwe@mail.ru", true)]
        public async void UserWithExistingEmailCantBeAdded(string userEmail, bool isModelValid)
        {
            var mock = new Mock<IStoreRepository>();

            mock.Setup(x => x.Users).Returns(new List<User>
            {
                new() {Email = "two@mail.ru"}, new() {Email = "ten@mail.ru"}
            }.AsQueryable());
            mock.Setup(x => x.Roles).Returns(new List<Role> {new("admin"), new("manager"), new("user")}.AsQueryable());
            mock.Setup(x => x.FindRole("user")).Returns(mock.Object.Roles.FirstOrDefault(x => x.Name == "user"));
            mock.Setup(x => x.CheckEmailAlreadyExists(userEmail))
                .Returns(mock.Object.Users.Any(x => x.Email == userEmail));

            var controller = new AccountController(mock.Object);
            await controller.Register(new UserRegistration
            {
                Name = "Петя",
                SecondName = "Иванов",
                Phone = "+79159999999",
                Email = userEmail,
                Password = "12345678",
                Role = new Role {Name = "user"}
            });
            Assert.Equal(isModelValid, controller.ModelState.IsValid);
        }
    }
}