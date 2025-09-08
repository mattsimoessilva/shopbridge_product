using ProductAPI.Models.Entities;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Product product);
        Task<bool> RemoveAsync(Guid id);
    }
}