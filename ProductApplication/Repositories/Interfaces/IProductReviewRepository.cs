using ProductApplication.Models;
using ProductApplication.Models.Entities;

namespace ProductApplication.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<ProductReview> AddAsync(ProductReview productReview);
        Task<IEnumerable<ProductReview>> GetAllAsync();
        Task<ProductReview?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(ProductReview productReview);
        Task<bool> DeleteAsync(Guid id);
    }
}