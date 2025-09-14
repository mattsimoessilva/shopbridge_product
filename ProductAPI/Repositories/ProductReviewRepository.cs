using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductAPI.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ProductAppDbContext _context;
        private readonly IMapper _mapper;
        public ProductReviewRepository(ProductAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductReview>  AddAsync(ProductReview entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.ProductReviews.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ProductReview>> GetAllAsync()
        {
            return await _context.ProductReviews
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductReview?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            return await _context.ProductReviews
                .AsNoTracking()
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(ProductReview updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var existing = await _context.ProductReviews.FirstOrDefaultAsync(p => p.Id == updated.Id);
            if (existing == null) return false;

            _mapper.Map(updated, existing);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            var entity = await _context.ProductReviews.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            _context.ProductReviews.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}