using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BooksStore.Models
{
    public static class SeedData
    {
        public static void EnsureBooksAdded(IApplicationBuilder applicationBuilder)
        {
            IStoreRepository repository = applicationBuilder.ApplicationServices.GetRequiredService<IStoreRepository>();

            if (repository.Books.Any() == false)
            {
                repository.AddBooksRange(
                    new Book()
                    {
                        Name = "У лукоморья дуб зелёный...",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о царе Салтане",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о золотом петушке",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о рыбаке и рыбке",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о попе и о работнике его Балде",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о мертвой царевне и о семи богатырях",
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    }
                );
            }
        }
    }
}
