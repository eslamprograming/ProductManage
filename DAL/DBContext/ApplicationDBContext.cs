using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductLog> ProductLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Product -> Category (Many-to-One)
            builder.Entity<Product>()
                .HasOne(p => p.Category) // Product has one Category
                .WithMany(c => c.Products) // Category has many Products
                .HasForeignKey(p => p.CategoryId) // Foreign key in Product
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // ProductLog -> Product (Many-to-One)
            builder.Entity<ProductLog>()
                .HasOne(pl => pl.Product) // ProductLog has one Product
                .WithMany() // No navigation property in Product
                .HasForeignKey(pl => pl.ProductId) // Foreign key in ProductLog
                .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes

            // Seed Data for Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" },
                new Category { Id = 3, Name = "Clothing" }
            );
        }
    }
}
