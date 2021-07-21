using BooksStore.Models;
using Xunit;

namespace BooksStore.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void CanChangeCategoryName()
        {
            var category = new Category {Name = "Сказки"};
            category.Name = "Рассказы";
            Assert.Equal("Рассказы", category.Name);
        }
    }
}