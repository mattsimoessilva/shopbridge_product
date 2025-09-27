using ProductApplication.Models.DTOs.ProductVariant;

namespace ProductApplication.Services.Interfaces
{
    public interface IProductVariantService
    {
        public Task<ProductVariantReadDTO> CreateAsync(ProductVariantCreateDTO dto);
        public Task<IEnumerable<ProductVariantReadDTO>> GetAllAsync();
        public Task<ProductVariantReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, ProductVariantUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
        Task<bool> ReserveStockAsync(Guid id, int quantity);
        Task<bool> ReleaseStockAsync(Guid id, int quantity);
        Task<bool> ReduceStockAsync(Guid id, int quantity);
    }
}