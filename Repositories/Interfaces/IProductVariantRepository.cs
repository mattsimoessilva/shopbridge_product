using ProductAPI.Models.Entities;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IProductVariantRepository
    {
        Task AddAsync(ProductVariant productVariant);
        Task<IEnumerable<ProductVariant>> GetAllAsync();
        Task<ProductVariant?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(ProductVariant productVariant);
        Task<bool> RemoveAsync(Guid id);
    }
}