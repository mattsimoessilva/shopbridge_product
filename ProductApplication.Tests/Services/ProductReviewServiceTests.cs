using AutoMapper;
using FluentAssertions;
using Moq;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories.Interfaces;
using ProductApplication.Services;

namespace ProductApplication.Tests.Services
{
    public class ProductReviewServiceTests
    {
        private readonly Mock<IProductReviewRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductReviewService _service;

        public ProductReviewServiceTests()
        {
            _repositoryMock = new Mock<IProductReviewRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductReviewService(_repositoryMock.Object, _mapperMock.Object);
        }

        #region CreateAsync Method.

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ProductReviewCreateDTO dto = null;

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("dto");

            _mapperMock.Verify(m => m.Map<ProductReview>(It.IsAny<ProductReviewCreateDTO>()), Times.Never);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductReview>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDTO_WhenDTOIsValid()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            
            var createDTO = new ProductReviewCreateDTO { ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var readDTO = new ProductReviewReadDTO { Id = entity.Id, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true };

            _mapperMock.Setup(m => m.Map<ProductReview>(createDTO)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ProductReviewReadDTO>(entity)).Returns(readDTO);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            var act = await _service.CreateAsync(createDTO);

            // Assert
            act.Should().BeEquivalentTo(readDTO);

            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductReview>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductReviewReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var createDTO = new ProductReviewCreateDTO { ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = createDTO.ProductId, UserId = createDTO.UserId, Rating = createDTO.Rating, Comment = createDTO.Comment, IsVerifiedPurchase = createDTO.IsVerifiedPurchase };

            _mapperMock.Setup(m => m.Map<ProductReview>(createDTO)).Returns(entity);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ThrowsAsync(new Exception("Repository failure."));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(createDTO);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoRecordsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProductReview>());

            // Act
            var act = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(act);
            Assert.Empty(act);

            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDTOs_WhenRecordsExist()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var entities = new List<ProductReview>
            {
                new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true },
                new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "somethingelse123", Rating = 1, Comment = "Not comfortable and not responsive!", IsVerifiedPurchase = true }
            };

            var dtos = new List<ProductReviewReadDTO>
            {
                new ProductReviewReadDTO { Id = entities[0].Id, ProductId = entities[0].ProductId, UserId = entities[0].UserId, Rating = entities[0].Rating, Comment = entities[0].Comment, CreatedAt = entities[0].CreatedAt, IsVerifiedPurchase = entities[0].IsVerifiedPurchase },
                new ProductReviewReadDTO { Id = entities[1].Id, ProductId = entities[1].ProductId, UserId = entities[1].UserId, Rating = entities[1].Rating, Comment = entities[1].Comment, CreatedAt = entities[1].CreatedAt, IsVerifiedPurchase = entities[1].IsVerifiedPurchase }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductReviewReadDTO>>(entities)).Returns(dtos);

            // Act
            var act = await _service.GetAllAsync();

            // Assert 
            act.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public async Task GetAllAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var exception = new Exception("Database connection failed.");

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.GetAllAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database connection failed.");
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid ID (Parameter 'id')");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDTO_WhenRecordExists()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var dto = new ProductReviewReadDTO { Id = entity.Id, ProductId = entity.ProductId, UserId = entity.UserId, Rating = entity.Rating, Comment = entity.Comment, CreatedAt = entity.CreatedAt, IsVerifiedPurchase = entity.IsVerifiedPurchase };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ProductReviewReadDTO>(entity)).Returns(dto);

            // Act
            var act = await _service.GetByIdAsync(id);

            // Assert
            act.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new Exception("Repository failure.");

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Repository failure.");
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldThrowArgumentException_WhenDTOisNullOrIdIsEmpty()
        {
            // Arrange
            ProductReviewUpdateDTO nullDTO = null;
            var emptyIdDTO = new ProductReviewUpdateDTO { Id = Guid.Empty, Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            // Act
            Func<Task> actWithNull = async () => await _service.UpdateAsync(nullDTO);
            Func<Task> actWithEmptyId = async () => await _service.UpdateAsync(emptyIdDTO);

            // Assert
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid update data.");
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid update data.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var updateDTO = new ProductReviewUpdateDTO { Id = Guid.NewGuid(), Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var existing = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 1, Comment = "Not comfortable and not responsive!", IsVerifiedPurchase = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(updateDTO.Id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(updateDTO, existing));
            _repositoryMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

            // Act
            var act = await _service.UpdateAsync(updateDTO);

            // Assert
            act.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var dto = new ProductReviewUpdateDTO { Id = Guid.NewGuid(), Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var existing = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 1, Comment = "Not comfortable and not responsive!", IsVerifiedPurchase = true };

            var exception = new Exception("Repository failure");

            _repositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(dto, existing));
            _repositoryMock.Setup(r => r.UpdateAsync(existing)).ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.UpdateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository failure");
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new Exception("Repository delete failed");

            _repositoryMock
                .Setup(r => r.DeleteAsync(id))
                .ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _service.DeleteAsync(id);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository delete failed");
        }

        #endregion
    }
}