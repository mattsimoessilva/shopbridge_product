using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.Entities;

namespace ProductApplication.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> PatchAsync(Guid id, Action<Product> patchAction);
    }
}