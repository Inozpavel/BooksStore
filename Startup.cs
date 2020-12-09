using BooksStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BooksStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(services => services.EnableEndpointRouting = false);
            services.AddSingleton<IStoreRepository, EFStoreRepository>();
            string connectionString = Configuration["LocalDbConnectionString"];
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(connectionString));

            services.AddAuthentication("Cookies")
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.IsEssential = true;
                });
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }

            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Store}/{action=Index}/{id?}");
            });

            SeedData.EnsureBooksAdded(app);
            SeedData.EnsureRolesAdded(app);
        }
    }
}
