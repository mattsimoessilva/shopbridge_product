using Microsoft.EntityFrameworkCore;
using ProductAPI.Models.Entities;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductAPI.Data
{
    public class ProductAppDbContext : DbContext
    {
        public ProductAppDbContext(DbContextOptions<ProductAppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductReview> ProductReviews => Set<ProductReview>();

        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



        }
    }
}