using AutoMapper;
using FluentAssertions;
using Moq;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services;
using System;

namespace ProductAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ProductService(_repositoryMock.Object, _mapperMock.Object);
        }

        #region CreateAsync Method.

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ProductCreateDTO dto = null;

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("dto");

            _mapperMock.Verify(m => m.Map<Product>(It.IsAny<ProductCreateDTO>()), Times.Never);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDTO_WhenDTOIsValid()
        {
            // Arrange
            var createDTO = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var entity = new Product { Id = Guid.NewGuid(), Name = createDTO.Name, ShortDescription = createDTO.ShortDescription, FullDescription = createDTO.FullDescription, Price = createDTO.Price, DiscountPrice = createDTO.DiscountPrice, IsActive = createDTO.IsActive, IsFeatured = createDTO.IsFeatured, SKU = createDTO.SKU, StockQuantity = createDTO.StockQuantity, MinimumStockThreshold = createDTO.MinimumStockThreshold, AllowBackorder = createDTO.AllowBackorder, Brand = createDTO.Brand, Category = createDTO.Category, Tags = createDTO.Tags, ImageUrl = createDTO.ImageUrl, ThumbnailUrl = createDTO.ThumbnailUrl, SeoTitle = createDTO.SeoTitle, Slug = createDTO.Slug, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            var readDTO = new ProductReadDTO { Id = Guid.NewGuid(), Name = entity.Name, ShortDescription = entity.ShortDescription, FullDescription = entity.FullDescription, Price = entity.Price, DiscountPrice = entity.DiscountPrice, IsActive = entity.IsActive, IsFeatured = entity.IsFeatured, SKU = entity.SKU, StockQuantity = entity.StockQuantity, MinimumStockThreshold = entity.MinimumStockThreshold, AllowBackorder = entity.AllowBackorder, Brand = entity.Brand, Category = entity.Category, Tags = entity.Tags, ImageUrl = entity.ImageUrl, ThumbnailUrl = entity.ThumbnailUrl, SeoTitle = entity.SeoTitle, Slug = entity.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _mapperMock.Setup(m => m.Map<Product>(createDTO)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ProductReadDTO>(entity)).Returns(readDTO);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            var act = await _service.CreateAsync(createDTO);

            // Assert
            act.Should().BeEquivalentTo(readDTO);

            _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<Product>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var dto = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var entity = new Product { Id = Guid.NewGuid(), Name = dto.Name, ShortDescription = dto.ShortDescription, FullDescription = dto.FullDescription, Price = dto.Price, DiscountPrice = dto.DiscountPrice, IsActive = dto.IsActive, IsFeatured = dto.IsFeatured, SKU = dto.SKU, StockQuantity = dto.StockQuantity, MinimumStockThreshold = dto.MinimumStockThreshold, AllowBackorder = dto.AllowBackorder, Brand = dto.Brand, Category = dto.Category, Tags = dto.Tags, ImageUrl = dto.ImageUrl, ThumbnailUrl = dto.ThumbnailUrl, SeoTitle = dto.SeoTitle, Slug = dto.Slug, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            
            _mapperMock.Setup(m => m.Map<Product>(dto)).Returns(entity);
            _repositoryMock.Setup(r => r.AddAsync(entity)).ThrowsAsync(new Exception("Repository failure."));

            // Act
            Func<Task> act = async () => await _service.CreateAsync(dto);

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
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

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
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", ShortDescription = "RGB backlit mechanical keyboard", FullDescription = "Durable mechanical keyboard with customizable RGB lighting and tactile switches.", Price = 249.99m, DiscountPrice = 199.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "KeyMaster", Category = "Accessories", Tags = "keyboard,mechanical,rgb", ImageUrl = "/images/products/mechanical-keyboard.jpg", ThumbnailUrl = "/images/products/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Tactile", Slug = "mechanical-keyboard", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() }
            };

            var mappedDTOs = new List<ProductReadDTO>
            {
                new ProductReadDTO { Id = products[0].Id, Name = products[0].Name, ShortDescription = products[0].ShortDescription, FullDescription = products[0].FullDescription, Price = products[0].Price, DiscountPrice = products[0].DiscountPrice, IsActive = products[0].IsActive, IsFeatured = products[0].IsFeatured, SKU = products[0].SKU, StockQuantity = products[0].StockQuantity, MinimumStockThreshold = products[0].MinimumStockThreshold, AllowBackorder = products[0].AllowBackorder, Brand = products[0].Brand, Category = products[0].Category, Tags = products[0].Tags, ImageUrl = products[0].ImageUrl, ThumbnailUrl = products[0].ThumbnailUrl, SeoTitle = products[0].SeoTitle, Slug = products[0].Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() },
                new ProductReadDTO { Id = products[1].Id, Name = products[1].Name, ShortDescription = products[1].ShortDescription, FullDescription = products[1].FullDescription, Price = products[1].Price, DiscountPrice = products[1].DiscountPrice, IsActive = products[1].IsActive, IsFeatured = products[1].IsFeatured, SKU = products[1].SKU, StockQuantity = products[1].StockQuantity, MinimumStockThreshold = products[1].MinimumStockThreshold, AllowBackorder = products[1].AllowBackorder, Brand = products[1].Brand, Category = products[1].Category, Tags = products[1].Tags, ImageUrl = products[1].ImageUrl, ThumbnailUrl = products[1].ThumbnailUrl, SeoTitle = products[1].SeoTitle, Slug = products[1].Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductReadDTO>>(products)).Returns(mappedDTOs);

            // Act
            var act = await _service.GetAllAsync();

            // Assert 
            act.Should().BeEquivalentTo(mappedDTOs);
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
            var id = Guid.NewGuid();

            var entity = new Product { Id = id, Name = "Gaming Headset", ShortDescription = "Surround sound gaming headset", FullDescription = "High-fidelity gaming headset with noise-canceling mic and RGB lighting.", Price = 199.99m, DiscountPrice = 149.99m, IsActive = true, IsFeatured = false, SKU = "GH-003", StockQuantity = 60, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "SoundBlaze", Category = "Accessories", Tags = "headset,gaming,surround", ImageUrl = "/images/products/gaming-headset.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-headset.jpg", SeoTitle = "Gaming Headset - Surround & RGB", Slug = "gaming-headset", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            var dto = new ProductReadDTO { Id = entity.Id, Name = entity.Name, ShortDescription = entity.ShortDescription, FullDescription = entity.FullDescription, Price = entity.Price, DiscountPrice = entity.DiscountPrice, IsActive = entity.IsActive, IsFeatured = entity.IsFeatured, SKU = entity.SKU, StockQuantity = entity.StockQuantity, MinimumStockThreshold = entity.MinimumStockThreshold, AllowBackorder = entity.AllowBackorder, Brand = entity.Brand, Category = entity.Category, Tags = entity.Tags, ImageUrl = entity.ImageUrl, ThumbnailUrl = entity.ThumbnailUrl, SeoTitle = entity.SeoTitle, Slug = entity.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ProductReadDTO>(entity)).Returns(dto);

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
            ProductUpdateDTO nullDTO = null;
            var emptyIdDTO = new ProductUpdateDTO { Id = Guid.Empty, Name = "Gaming Monitor", ShortDescription = "High refresh rate monitor", FullDescription = "27-inch 144Hz monitor with HDR support", Price = 899.99m, DiscountPrice = 749.99m, IsActive = true, IsFeatured = false, StockQuantity = 40, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "ViewMax", Category = "Displays", Tags = "monitor,gaming,144hz", ImageUrl = "/images/products/gaming-monitor.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-monitor.jpg", SeoTitle = "Gaming Monitor - 144Hz HDR", Slug = "gaming-monitor" };

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
            var updateDTO = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Gaming Monitor", ShortDescription = "High refresh rate monitor", FullDescription = "27-inch 144Hz monitor with HDR support", Price = 899.99m, DiscountPrice = 749.99m, IsActive = true, IsFeatured = false, StockQuantity = 40, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "ViewMax", Category = "Displays", Tags = "monitor,gaming,144hz", ImageUrl = "/images/products/gaming-monitor.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-monitor.jpg", SeoTitle = "Gaming Monitor - 144Hz HDR", Slug = "gaming-monitor" };
            var existing = new Product { Id = updateDTO.Id, Name = "Old Name", ShortDescription = "Old Desc", FullDescription = "Old Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "OldBrand", Category = "OldCategory", Tags = "old,tags", ImageUrl = "/images/old.jpg", ThumbnailUrl = "/images/thumbs/old.jpg", SeoTitle = "Old SEO", Slug = "old-slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

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
            var dto = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Gaming Monitor", ShortDescription = "High refresh rate monitor", FullDescription = "27-inch 144Hz monitor with HDR support", Price = 899.99m, DiscountPrice = 749.99m, IsActive = true, IsFeatured = false, StockQuantity = 40, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "ViewMax", Category = "Displays", Tags = "monitor,gaming,144hz", ImageUrl = "/images/products/gaming-monitor.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-monitor.jpg", SeoTitle = "Gaming Monitor - 144Hz HDR", Slug = "gaming-monitor" };

            var existing = new Product { Id = dto.Id, Name = "Old Name", ShortDescription = "Old Desc", FullDescription = "Old Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "OldBrand", Category = "OldCategory", Tags = "old,tags", ImageUrl = "/images/old.jpg", ThumbnailUrl = "/images/thumbs/old.jpg", SeoTitle = "Old SEO", Slug = "old-slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

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
            var act = await _service.DeleteAsync(id);

            // Assert
            act.Should().BeTrue();
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