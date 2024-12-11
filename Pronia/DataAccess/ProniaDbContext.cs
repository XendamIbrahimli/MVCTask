
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DataAccess
{
    public class ProniaDbContext:IdentityDbContext<User>
    {

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public ProniaDbContext(DbContextOptions<ProniaDbContext> opt):base(opt)
        {

        }

    }
}
