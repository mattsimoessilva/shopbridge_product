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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



        }
    }
}