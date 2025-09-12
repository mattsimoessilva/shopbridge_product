using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;
using ProductAPI.Models.Entities;

namespace ProductAPI.Services.Interfaces
{
    public interface IProductReviewService
    {
        public Task<ProductReviewReadDTO> CreateAsync(ProductReviewCreateDTO dto);
        public Task<IEnumerable<ProductReviewReadDTO>> GetAllAsync();
        public Task<ProductReviewReadDTO?> GetByIdAsync(Guid id);
        public Task<bool> UpdateAsync(ProductReviewUpdateDTO dto);
        public Task<bool> DeleteAsync(Guid id);
    }
}