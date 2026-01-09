using Microsoft.EntityFrameworkCore;
using AnnFlowerProject.Models;

namespace AnnFlowerProject.Data
{
    public class FlowerShopDbContext : DbContext
    {
        public FlowerShopDbContext(DbContextOptions<FlowerShopDbContext> options)
            : base(options)
        {
        }

        // DbSets cho các b?ng
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // C?u h?nh relationship User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // C?u h?nh relationship Product - Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
