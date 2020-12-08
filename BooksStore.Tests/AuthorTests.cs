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
                Name = "������ �. �."
            };
            author.Name = "������� �.�.";
            Assert.Equal("������� �.�.", author.Name);
        }
    }
}
