using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;

namespace Pronia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ProniaDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("local"));
            });                                                          

            builder.Services.AddIdentity<User, IdentityRole>(opt=>
            {
                opt.Password.RequiredLength = 2;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase =false ;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts= 1; 
                opt.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromSeconds(20);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ProniaDbContext>();



            var app = builder.Build();
            
            
            app.UseStaticFiles();

            app.MapControllerRoute(name: "register",
                pattern: "register",
                defaults: new { controller = "Account", action = "Register" });

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default", 
                pattern: "{controller=Home}/{action=Index}/{Id?}");



            app.Run();
        }
    }
}
