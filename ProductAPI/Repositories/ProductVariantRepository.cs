using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ProductAppDbContext _context;

        public ProductVariantRepository(ProductAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductVariant productVariant)
        {
            await _context.ProductVariants.AddAsync(productVariant);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdAsync(Guid id)
        {
            return await _context.ProductVariants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(ProductVariant updatedProductVariant)
        {
            var existingProductVariant = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id == updatedProductVariant.Id);
            if (existingProductVariant == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var productVariant = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id == id);
            if (productVariant == null) return false;

            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}