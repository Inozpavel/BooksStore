using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BooksStore.Models
{
    public static class SeedData
    {
        public static void EnsureBooksAdded(IApplicationBuilder applicationBuilder)
        {
            StoreContext context = applicationBuilder.ApplicationServices.GetRequiredService<StoreContext>();
            if (context.Authors.Any() == false)
            {
                context.Authors.Add(new Author("Пушкин А. С."));
            }

            if (context.Categories.Any() == false)
            {
                context.Categories.Add(new Category("Сказки"));
            }

            if (context.Books.Any() == false)
            {

                context.Books.AddRange(
                    new Book()
                    {
                        Name = "У лукоморья дуб зелёный...",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о царе Салтане",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о золотом петушке",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о рыбаке и рыбке",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о попе и о работнике его Балде",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    },
                    new Book()
                    {
                        Name = "Сказка о мертвой царевне и о семи богатырях",
                        Author = context.Authors.Find("Пушкин А. С."),
                        Category = context.Categories.Find("Сказки"),
                    }
                );
            }
            context.SaveChanges();
        }
    }
}
