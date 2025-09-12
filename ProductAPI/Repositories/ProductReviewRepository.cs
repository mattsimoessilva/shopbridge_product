using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ProductAppDbContext _context;

        public ProductReviewRepository(ProductAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductReview productReview)
        {
            await _context.ProductReviews.AddAsync(productReview);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductReview>> GetAllAsync()
        {
            return await _context.ProductReviews
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductReview?> GetByIdAsync(Guid id)
        {
            return await _context.ProductReviews
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(ProductReview updatedProductReview)
        {
            var existingProductReview = await _context.ProductReviews.FirstOrDefaultAsync(p => p.Id == updatedProductReview.Id);
            if (existingProductReview == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var productReview = await _context.ProductReviews.FirstOrDefaultAsync(p => p.Id == id);
            if (productReview == null) return false;

            _context.ProductReviews.Remove(productReview);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}