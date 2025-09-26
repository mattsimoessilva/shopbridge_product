using AutoMapper;
using FluentAssertions;
using Moq;
using ProductApplication.Models.DTOs.ProductVariant;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories.Interfaces;
using ProductApplication.Services;

namespace ProductApplication.Tests.Services
{
    public class ProductVariantServiceTests
    {
        private readonly Mock<IProductVariantRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductVariantService _service;

        public ProductVariantServiceTests()
        {
            _repositoryMock = new Mock<IProductVariantRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductVariantService(_repositoryMock.Object, _mapperMock.Object);
        }

        #region CreateAsync Method.

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ProductVariantCreateDTO dto = null;

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("dto");

            _mapperMock.Verify(m => m.Map<ProductVariant>(It.IsAny<ProductVariantCreateDTO>()), Times.Never);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductVariant>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDTO_WhenDTOIsValid()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var createDTO = new ProductVariantCreateDTO { ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var entity = new ProductVariant { Id = id, Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var readDTO = new ProductVariantReadDTO { Id = id, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _mapperMock.Setup(m => m.Map<ProductVariant>(createDTO)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ProductVariantReadDTO>(entity)).Returns(readDTO);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            var act = await _service.CreateAsync(createDTO);

            // Assert
            act.Should().BeEquivalentTo(readDTO);

            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductVariant>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductVariantReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            
            var id = Guid.NewGuid();
            var createDTO = new ProductVariantCreateDTO { ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var entity = new ProductVariant { Id = id, Product = reference, ProductId = createDTO.ProductId, VariantName = createDTO.VariantName, Color = createDTO.Color, Size = createDTO.Size, Price = createDTO.Price, StockQuantity = createDTO.StockQuantity, ImageUrl = createDTO.ImageUrl, IsActive = createDTO.IsActive };

            _mapperMock.Setup(m => m.Map<ProductVariant>(createDTO)).Returns(entity);
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
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProductVariant>());

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

            var entities = new List<ProductVariant>
            {
                new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true },
                new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Hel Edition", Color = "White", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-white.jpg", IsActive = true }
            };

            var dtos = new List<ProductVariantReadDTO>
            {
                new ProductVariantReadDTO { Id = entities[0].Id, ProductId = entities[0].ProductId, VariantName = entities[0].VariantName, Color = entities[0].Color, Size = entities[0].Size, Price = entities[0].Price, StockQuantity = entities[0].StockQuantity, ImageUrl = entities[0].ImageUrl, IsActive = entities[0].IsActive },
                new ProductVariantReadDTO { Id = entities[1].Id, ProductId = entities[1].ProductId, VariantName = entities[1].VariantName, Color = entities[1].Color, Size = entities[1].Size, Price = entities[1].Price, StockQuantity = entities[1].StockQuantity, ImageUrl = entities[1].ImageUrl, IsActive = entities[1].IsActive }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductVariantReadDTO>>(entities)).Returns(dtos);

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
            var entity = new ProductVariant { Id = id, Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var dto = new ProductVariantReadDTO { Id = entity.Id, ProductId = entity.ProductId, VariantName = entity.VariantName, Color = entity.Color, Size = entity.Size, Price = entity.Price, StockQuantity = entity.StockQuantity, ImageUrl = entity.ImageUrl, IsActive = entity.IsActive };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ProductVariantReadDTO>(entity)).Returns(dto);

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
            ProductVariantUpdateDTO nullDTO = null;
            var emptyIdDTO = new ProductVariantUpdateDTO { Id = Guid.Empty, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

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

            var updateDTO = new ProductVariantUpdateDTO { Id = Guid.NewGuid(), VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var existing = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Hel Edition", Color = "White", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-white.jpg", IsActive = true };

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

            var dto = new ProductVariantUpdateDTO { Id = Guid.NewGuid(), VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var existing = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Hel Edition", Color = "White", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-white.jpg", IsActive = true };

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

        #region ReserveStockAsync Method.

        [Fact]
        public async Task ReserveStockAsync_ShouldReturnTrue_WhenReservationIsSuccessful()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 5;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 2, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var result = await _service.ReserveStockAsync(id, quantity);

            // Arrange
            result.Should().BeTrue();
            entity.ReservedStockQuantity.Should().Be(7);
            _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        }

        [Fact]
        public async Task ReserveStockAsync_ShouldThrowInvalidOperationException_WhenNotEnoughStock()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 10;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 5, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            Func<Task> act = async () => await _service.ReserveStockAsync(id, quantity);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Not enough stock available to reserve.");
        }

        [Fact]
        public async Task ReserveStockAsync_ShouldReturnFalse_WhenEntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 5;

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductVariant)null);

            // Act
            var result = await _service.ReserveStockAsync(id, quantity);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ProductVariant>()), Times.Never);
        }

        #endregion

        #region ReleaseStockAsync Method.

        [Fact]
        public async Task ReleaseStockAsync_ShouldReturnTrue_WhenReleaseIsSuccessful()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 3;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 5, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var result = await _service.ReleaseStockAsync(id, quantity);

            // Assert
            result.Should().BeTrue();
            entity.ReservedStockQuantity.Should().Be(2);
            _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        }

        [Fact]
        public async Task ReleaseStockAsync_ShouldThrowInvalidOperationException_WhenReleasingMoreThanReserved()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 10;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 5, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            Func<Task> act = async () => await _service.ReleaseStockAsync(id, quantity);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Cannot release more stock than is reserved.");
        }

        [Fact]
        public async Task ReleaseStockAsync_ShouldReturnFalse_WhenEntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var quantity = 3;

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductVariant)null);

            // Act
            var result = await _service.ReleaseStockAsync(id, quantity);

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ProductVariant>()), Times.Never);
        }

        #endregion

        #region ReduceStockAsync Method.

        [Fact]
        public async Task ReduceStockAsync_ShouldReturnTrue_WhenReductionIsSuccessful()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 3;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 5, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var result = await _service.ReduceStockAsync(id, quantity);

            // Assert
            result.Should().BeTrue();
            entity.StockQuantity.Should().Be(7);
            entity.ReservedStockQuantity.Should().Be(2);
            _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        }

        [Fact]
        public async Task ReduceStockAsync_ShouldThrowInvalidOperationException_WhenReservedStockIsTooLow()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 5;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 10, ReservedStockQuantity = 3, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            Func<Task> act = async () => await _service.ReduceStockAsync(id, quantity);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Cannot reduce more stock than is reserved.");
        }

        [Fact]
        public async Task ReduceStockAsync_ShouldThrowInvalidOperationException_WhenTotalStockIsTooLow()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var id = Guid.NewGuid();
            var quantity = 10;
            var entity = new ProductVariant { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 8, ReservedStockQuantity = 10, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            Func<Task> act = async () => await _service.ReduceStockAsync(id, quantity);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Not enough total stock to reduce.");
        }

        #endregion
    }
}