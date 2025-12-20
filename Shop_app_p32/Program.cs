using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shop_app_p32.DbContext;
using Microsoft.AspNetCore.Identity;

namespace Shop_app_p32
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Services
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
