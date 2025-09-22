using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.Entities;

namespace ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductReadDTO> CreateAsync(ProductCreateDTO dto);
        public Task<IEnumerable<ProductReadDTO>> GetAllAsync();
        public Task<ProductReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(ProductUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);

        Task<bool> ReserveStockAsync(Guid productId, int quantity);

        Task<bool> ReleaseStockAsync(Guid productId, int quantity);

        Task<bool> ReduceStockAsync(Guid productId, int quantity);

    }
}