using ProductAPI.Models.DTOs.ProductVariant;

namespace ProductAPI.Services.Interfaces
{
    public interface IProductVariantService
    {
        public Task<ProductVariantReadDTO> CreateAsync(ProductVariantCreateDTO dto);
        public Task<IEnumerable<ProductVariantReadDTO>> GetAllAsync();
        public Task<ProductVariantReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(ProductVariantUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
    }
}