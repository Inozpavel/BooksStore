namespace BooksStore.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public byte[] Image { get; set; }
    }
}