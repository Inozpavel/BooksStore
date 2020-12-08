using BooksStore.Models;
using Xunit;

namespace BooksStore.Tests
{
    public class AuthorTests
    {
        [Fact]
        public void CanChangeAuthorName()
        {
            Author author = new Author()
            {
                Name = "Пушкин А. С."
            };
            author.Name = "Толстой Л.Н.";
            Assert.Equal("Толстой Л.Н.", author.Name);
        }
    }
}
