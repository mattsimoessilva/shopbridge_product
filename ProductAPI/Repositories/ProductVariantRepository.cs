using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using AutoMapper;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ProductAppDbContext _context;
        private readonly IMapper _mapper;
        public ProductVariantRepository(ProductAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductVariant> AddAsync(ProductVariant entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.ProductVariants.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            return await _context.ProductVariants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(ProductVariant updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var existing = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id == updated.Id);
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

            var entity = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            _context.ProductVariants.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}