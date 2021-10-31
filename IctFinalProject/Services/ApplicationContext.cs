using IctFinalProject.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace IctFinalProject.Services
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInCart> ProductsInCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(f => f.OrderCode)
                .ValueGeneratedOnAdd();
        }
    }
}