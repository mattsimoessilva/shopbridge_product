using AutoMapper;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductReadDTO> CreateAsync(ProductCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var product = _mapper.Map<Product>(dto);
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;

            await _productRepository.AddAsync(product);

            return _mapper.Map<ProductReadDTO>(product);
        }

        public async Task<IEnumerable<ProductReadDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (products == null || !products.Any())
                return Enumerable.Empty<ProductReadDTO>();

            var result = _mapper.Map<IEnumerable<ProductReadDTO>>(products);

            return result;
        }

        public async Task<ProductReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return null;

            return _mapper.Map<ProductReadDTO>(product);
        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid update data.");

            var existingProduct = await _productRepository.GetByIdAsync(dto.Id);
            if (existingProduct == null)
                return false;

            _mapper.Map(dto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existingProduct);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id) => await _productRepository.DeleteAsync(id);

        public async Task<bool> ReserveStockAsync(Guid productId, int quantity)
        {
            if (productId == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid product ID or quantity.");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            var availableStock = product.StockQuantity - product.ReservedStockQuantity;
            if (availableStock < quantity)
                throw new InvalidOperationException("Not enough stock available to reserve.");

            product.ReservedStockQuantity += quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> ReleaseStockAsync(Guid productId, int quantity)
        {
            if (productId == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid product ID or quantity.");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            if (product.ReservedStockQuantity < quantity)
                throw new InvalidOperationException("Cannot release more stock than is reserved.");

            product.ReservedStockQuantity -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> ReduceStockAsync(Guid productId, int quantity)
        {
            if (productId == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid product ID or quantity.");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            if (product.ReservedStockQuantity < quantity)
                throw new InvalidOperationException("Cannot reduce more stock than is reserved.");

            if (product.StockQuantity < quantity)
                throw new InvalidOperationException("Not enough total stock to reduce.");

            product.StockQuantity -= quantity;
            product.ReservedStockQuantity -= quantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}