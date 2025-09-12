using AutoMapper;
using ProductAPI.Models;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IMapper _mapper;

        public ProductReviewService(IProductReviewRepository productReviewRepository, IMapper mapper)
        {
            _productReviewRepository = productReviewRepository;
            _mapper = mapper;
        }

        public async Task<ProductReviewReadDTO> CreateAsync(ProductReviewCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var productReview = _mapper.Map<ProductReview>(dto);
            productReview.Id = Guid.NewGuid();
            productReview.CreatedAt = DateTime.UtcNow;

            await _productReviewRepository.AddAsync(productReview);

            return _mapper.Map<ProductReviewReadDTO>(productReview);
        }

        public async Task<IEnumerable<ProductReviewReadDTO>> GetAllAsync()
        {
            var productReviews = await _productReviewRepository.GetAllAsync();

            if (productReviews == null || !productReviews.Any())
                return Enumerable.Empty<ProductReviewReadDTO>();

            var result = _mapper.Map<IEnumerable<ProductReviewReadDTO>>(productReviews);

            return result;
        }

        public async Task<ProductReviewReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid product review ID", nameof(id));

            var productReview = await _productReviewRepository.GetByIdAsync(id);

            if (productReview is null)
                return null;

            return _mapper.Map<ProductReviewReadDTO>(productReview);
        }

        public async Task<bool> UpdateAsync(ProductReviewUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid product review update data.");

            var existingProductReview = await _productReviewRepository.GetByIdAsync(dto.Id);
            if (existingProductReview == null)
                return false;

            _mapper.Map(dto, existingProductReview);

            await _productReviewRepository.UpdateAsync(existingProductReview);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id) => await _productReviewRepository.DeleteAsync(id);
    }
}