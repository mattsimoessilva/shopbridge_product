using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;

namespace ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductAppDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ProductAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> AddAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return product;
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

        public async Task<bool> UpdateAsync(Product updatedProduct)
        {
            if (updatedProduct == null)
                throw new ArgumentNullException(nameof(updatedProduct));

            var existingProduct = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

            if (existingProduct == null)
                return false;

            _mapper.Map(updatedProduct, existingProduct);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(
                    "Id cannot be an empty GUID",
                    nameof(id));

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}