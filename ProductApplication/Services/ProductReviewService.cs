using AutoMapper;
using ProductApplication.Models;
using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories.Interfaces;
using ProductApplication.Services.Interfaces;

namespace ProductApplication.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _repository;
        private readonly IMapper _mapper;

        public ProductReviewService(IProductReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductReviewReadDTO> CreateAsync(ProductReviewCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var productReview = _mapper.Map<ProductReview>(dto);
            productReview.Id = Guid.NewGuid();
            productReview.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(productReview);

            return _mapper.Map<ProductReviewReadDTO>(productReview);
        }

        public async Task<IEnumerable<ProductReviewReadDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ProductReviewReadDTO>();

            return _mapper.Map<IEnumerable<ProductReviewReadDTO>>(entities);
        }

        public async Task<ProductReviewReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var entity = await _repository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<ProductReviewReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(ProductReviewUpdateDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                throw new ArgumentException("Invalid update data.");

            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);

            await _repository.UpdateAsync(existing);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            await _repository.DeleteAsync(id);

            return true;
        }
    }
}