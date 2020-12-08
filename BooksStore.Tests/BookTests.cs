using BooksStore.Models;
using Xunit;

namespace BooksStore.Tests
{
    public class BookTests
    {
        [Fact]
        public void CanChangeBookName()
        {
            Book book = new Book()
            {
                Name = "Сказка о царе Салтане"
            };
            book.Name = "У лукоморья дуб зеленый...";
            Assert.Equal("У лукоморья дуб зеленый...", book.Name);
        }
    }
}
