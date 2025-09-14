using AutoMapper;
using ProductAPI.Models;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductVariant;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IMapper _mapper;

        public ProductVariantService(IProductVariantRepository productVariantRepository, IMapper mapper)
        {
            _productVariantRepository = productVariantRepository;
            _mapper = mapper;
        }

        public async Task<ProductVariantReadDTO> CreateAsync(ProductVariantCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var productVariant = _mapper.Map<ProductVariant>(dto);
            productVariant.Id = Guid.NewGuid();

            await _productVariantRepository.AddAsync(productVariant);

            return _mapper.Map<ProductVariantReadDTO>(productVariant);
        }

        public async Task<IEnumerable<ProductVariantReadDTO>> GetAllAsync()
        {
            var productVariants = await _productVariantRepository.GetAllAsync();

            if (productVariants == null || !productVariants.Any())
                return Enumerable.Empty<ProductVariantReadDTO>();

            var result = _mapper.Map<IEnumerable<ProductVariantReadDTO>>(productVariants);

            return result;
        }

        public async Task<ProductVariantReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var productVariant = await _productVariantRepository.GetByIdAsync(id);

            if (productVariant is null)
                return null;

            return _mapper.Map<ProductVariantReadDTO>(productVariant);
        }

        public async Task<bool> UpdateAsync(ProductVariantUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid update data.");

            var existingProductVariant = await _productVariantRepository.GetByIdAsync(dto.Id);
            if (existingProductVariant == null)
                return false;

            _mapper.Map(dto, existingProductVariant);

            await _productVariantRepository.UpdateAsync(existingProductVariant);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id) => await _productVariantRepository.DeleteAsync(id);
    }
}