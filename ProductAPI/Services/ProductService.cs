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
                throw new ArgumentException("Invalid product ID", nameof(id));

            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return null;

            return _mapper.Map<ProductReadDTO>(product);
        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid product update data.");

            var existingProduct = await _productRepository.GetByIdAsync(dto.Id);
            if (existingProduct == null)
                return false;

            _mapper.Map(dto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existingProduct);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id) => await _productRepository.DeleteAsync(id);
    }
}