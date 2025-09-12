using ProductAPI.Models;
using ProductAPI.Models.Entities;

namespace ProductAPI.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task AddAsync(ProductReview productReview);
        Task<IEnumerable<ProductReview>> GetAllAsync();
        Task<ProductReview?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(ProductReview productReview);
        Task<bool> RemoveAsync(Guid id);
    }
}