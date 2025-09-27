using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.Entities;

namespace ProductApplication.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductReadDTO> CreateAsync(ProductCreateDTO dto);
        public Task<IEnumerable<ProductReadDTO>> GetAllAsync();
        public Task<ProductReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, ProductUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
        Task<bool> ReserveStockAsync(Guid id, int quantity);
        Task<bool> ReleaseStockAsync(Guid id, int quantity);
        Task<bool> ReduceStockAsync(Guid id, int quantity);

    }
}