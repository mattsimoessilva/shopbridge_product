using AutoMapper;
using ProductApplication.Models;
using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.DTOs.ProductVariant;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories;
using ProductApplication.Repositories.Interfaces;
using ProductApplication.Services.Interfaces;

namespace ProductApplication.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _repository;
        private readonly IMapper _mapper;

        public ProductVariantService(IProductVariantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductVariantReadDTO> CreateAsync(ProductVariantCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var entity = _mapper.Map<ProductVariant>(dto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(entity);

            return _mapper.Map<ProductVariantReadDTO>(entity);
        }

        public async Task<IEnumerable<ProductVariantReadDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ProductVariantReadDTO>();

            return _mapper.Map<IEnumerable<ProductVariantReadDTO>>(entities);
        }

        public async Task<ProductVariantReadDTO?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid ID", nameof(id));

            var entity = await _repository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<ProductVariantReadDTO>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, ProductVariantUpdateDTO dto)
        {
            if (dto == null || id == Guid.Empty)
                throw new ArgumentException("Invalid update data.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

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

        public async Task<bool> ReserveStockAsync(Guid id, int quantity)
        {
            if (id == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid entity ID or quantity.");

            return await _repository.PatchAsync(id, entity =>
            {
                var available = entity.StockQuantity - entity.ReservedStockQuantity;
                if (available < quantity)
                    throw new InvalidOperationException("Not enough stock available to reserve.");

                entity.ReservedStockQuantity += quantity;
                entity.UpdatedAt = DateTime.UtcNow;
            });
        }

        public async Task<bool> ReleaseStockAsync(Guid id, int quantity)
        {
            if (id == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid entity ID or quantity.");

            return await _repository.PatchAsync(id, entity =>
            {
                if (entity.ReservedStockQuantity < quantity)
                    throw new InvalidOperationException("Cannot release more stock than is reserved.");

                entity.ReservedStockQuantity -= quantity;
                entity.UpdatedAt = DateTime.UtcNow;
            });
        }

        public async Task<bool> ReduceStockAsync(Guid id, int quantity)
        {
            if (id == Guid.Empty || quantity <= 0)
                throw new ArgumentException("Invalid entity ID or quantity.");

            return await _repository.PatchAsync(id, entity =>
            {
                if (entity.ReservedStockQuantity < quantity)
                    throw new InvalidOperationException("Cannot reduce more stock than is reserved.");

                if (entity.StockQuantity < quantity)
                    throw new InvalidOperationException("Not enough total stock to reduce.");

                entity.StockQuantity -= quantity;
                entity.ReservedStockQuantity -= quantity;
                entity.UpdatedAt = DateTime.UtcNow;
            });
        }
    }
}