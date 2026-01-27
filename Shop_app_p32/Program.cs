using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop_app_p32.DbContext;
using Shop_app_p32.Services;

namespace Shop_app_p32
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Services
            builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
            builder.Services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<ProductContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // Require email confirmation for login
                options.SignIn.RequireConfirmedEmail = true;

                // Set custom password requirements
                options.Password.RequireDigit = false; // No digit required
                options.Password.RequireNonAlphanumeric = false; // No special characters required
                options.Password.RequiredLength = 4; // Minimum length of 4 characters
                options.Password.RequireUppercase = false; // No uppercase letter required
                options.Password.RequireLowercase = false; // No lowercase letter required
                options.Password.RequiredUniqueChars = 0; // No unique characters required
            })
            .AddRoles<IdentityRole>() // Add role management support to Identity
            .AddEntityFrameworkStores<UserContext>(); // Store Identity data in UserContext with Entity Framework
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ShopApp.Auth";

                // ⏳ життєвий цикл cookie
                options.ExpireTimeSpan = TimeSpan.FromHours(2);

                // 🔁 sliding expiration
                options.SlidingExpiration = true;

                // 🔐 безпека
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                // 🚪 редіректи
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Denied";
            });



            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            //Middlewere

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}
