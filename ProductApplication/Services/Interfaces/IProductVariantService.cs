using ProductApplication.Models.DTOs.ProductVariant;

namespace ProductApplication.Services.Interfaces
{
    public interface IProductVariantService
    {
        public Task<ProductVariantReadDTO> CreateAsync(ProductVariantCreateDTO dto);
        public Task<IEnumerable<ProductVariantReadDTO>> GetAllAsync();
        public Task<ProductVariantReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(ProductVariantUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
        Task<bool> ReserveStockAsync(Guid productId, int quantity);
        Task<bool> ReleaseStockAsync(Guid productId, int quantity);
        Task<bool> ReduceStockAsync(Guid productId, int quantity);
    }
}