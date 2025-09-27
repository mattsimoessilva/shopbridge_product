using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductApplication.Data;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories.Interfaces;

namespace ProductApplication.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> AddAsync(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Products.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(Product updated)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var existing = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == updated.Id);

            if (existing == null)
                return false;

            _mapper.Map(updated, existing);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchAsync(Guid id, Action<Product> patchAction)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null) return false;

            patchAction(entity);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}