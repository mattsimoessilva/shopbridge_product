using Microsoft.EntityFrameworkCore;
using ProductApplication.Models.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductApplication.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



        }
    }
}