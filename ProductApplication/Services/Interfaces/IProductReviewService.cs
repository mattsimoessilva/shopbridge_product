using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Models.DTOs.ProductVariant;
using ProductApplication.Models.Entities;

namespace ProductApplication.Services.Interfaces
{
    public interface IProductReviewService
    {
        public Task<ProductReviewReadDTO> CreateAsync(ProductReviewCreateDTO dto);
        public Task<IEnumerable<ProductReviewReadDTO>> GetAllAsync();
        public Task<ProductReviewReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, ProductReviewUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
    }
}