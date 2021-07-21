using System.Security.Claims;
using BooksStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BooksStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IStoreRepository, EfStoreRepository>();
            string connectionString = Configuration["LocalDbConnectionString"];
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(connectionString));

            services.AddAuthentication("Cookies")
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.IsEssential = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanChangeOrAddItems",
                    policy =>
                        policy.RequireAssertion(x =>
                            x.User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "admin")
                            || x.User.HasClaim(ClaimsIdentity.DefaultRoleClaimType,
                                "manager")));

                options.AddPolicy("CanRemoveItems",
                    policy => policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Store}/{action=Index}/{id?}");
                routes.MapRoute("profile", "{controller=Account}/{action=Profile}");
            });

            SeedData.EnsureRolesAdded(app);
            SeedData.EnsureCategoriesAdded(app);
            SeedData.EnsureAuthorsAdded(app);
            SeedData.EnsureBooksAdded(app);
        }
    }
}