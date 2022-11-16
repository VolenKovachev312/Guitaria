using Guitaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            User admin = new User()
            {
                Email = "admin@gmail.com",
                UserName = "Administrator"
            };
            
            base.OnModelCreating(builder);
        }
        
    }
}