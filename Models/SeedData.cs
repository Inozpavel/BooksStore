using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BooksStore.Models
{
    public static class SeedData
    {
        public static void EnsureRolesAdded(IApplicationBuilder applicationBuilder)
        {
            IStoreRepository repository = applicationBuilder.ApplicationServices.GetRequiredService<IStoreRepository>();

            if (repository.Roles.Any() == false)
            {
                repository.AddRole(new Role() { Name = "admin" });
                repository.AddRole(new Role() { Name = "manager" });
                repository.AddRole(new Role() { Name = "user" });
            }
        }

        public static void EnsureAuthorsAdded(IApplicationBuilder applicationBuilder)
        {
            IStoreRepository repository = applicationBuilder.ApplicationServices.GetRequiredService<IStoreRepository>();

            if (repository.Authors.Any() == false)
            {
                repository.AddAuthorsRange(
                    new Author("Толстой Л. Н."),
                    new Author("Гоголь Н. В."),
                    new Author("Лесков Н. С."),
                    new Author("Чехов А. П."),
                    new Author("Пушкин А. С."),
                    new Author("Салтыков-Шедрин М. Е.")
                );
            }
        }

        public static void EnsureCategoriesAdded(IApplicationBuilder applicationBuilder)
        {
            IStoreRepository repository = applicationBuilder.ApplicationServices.GetRequiredService<IStoreRepository>();

            if (repository.Categories.Any() == false)
            {
                repository.AddCategoriesRange(
                    new Category("Рассказы"),
                    new Category("Басни"),
                    new Category("Романы"),
                    new Category("Комедии"),
                    new Category("Повести"),
                    new Category("Детективы"),
                    new Category("Трагедии"),
                    new Category("Ужасы"),
                    new Category("Сказки"),
                    new Category("Поэмы"),
                    new Category("Эпопеи")
                );
            }
        }

        public static void EnsureBooksAdded(IApplicationBuilder applicationBuilder)
        {
            IStoreRepository repository = applicationBuilder.ApplicationServices.GetRequiredService<IStoreRepository>();

            if (repository.Books.Any() == false)
            {
                repository.AddBooksRange(
                    new Book("Левша", 1300)
                    {
                        Author = new Author("Лесков Н. С."),
                        Category = new Category("Повести"),
                    },
                    new Book("Мертвые души", 900)
                    {
                        Author = new Author("Гоголь Н. В."),
                        Category = new Category("Поэмы"),
                    },
                    new Book("Анна Каренина", 800)
                    {
                        Author = new Author("Толстой Л. Н."),
                        Category = new Category("Романы"),
                    },
                    new Book("У лукоморья дуб зелёный", 800)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book("Война и мир", 1000)
                    {
                        Author = new Author("Толстой Л. Н."),
                        Category = new Category("Эпопеи"),
                    },
                    new Book("Сказка о царе Салтане", 1200)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book("Шинель", 500)
                    {
                        Author = new Author("Гоголь Н. В."),
                        Category = new Category("Повести"),
                    },
                    new Book("Сказка о золотом петушке", 600)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book("Сказка о рыбаке и рыбке", 1400)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book("Нос", 600)
                    {
                        Author = new Author("Гоголь Н. В."),
                        Category = new Category("Повести"),
                    },
                    new Book("Сказка о попе и о работнике его Балде", 2000)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки"),
                    },
                    new Book("Сказка о мертвой царевне и о семи богатырях", 900)
                    {
                        Author = new Author("Пушкин А. С."),
                        Category = new Category("Сказки")
                    }
                );
            }
        }
    }
}
