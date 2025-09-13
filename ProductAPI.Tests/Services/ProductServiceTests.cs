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
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        #region CreateAsync Method.

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ProductCreateDTO createDTO = null;

            // Act
            Func<Task> act = async () => await _productService.CreateAsync(createDTO);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("dto");

            _mapperMock.Verify(m => m.Map<Product>(It.IsAny<ProductCreateDTO>()), Times.Never);
            _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
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
            _productRepositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);

            // Act
            var result = await _productService.CreateAsync(createDTO);

            // Assert
            result.Should().BeEquivalentTo(readDTO);

            _productRepositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<Product>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var createDTO = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var mappedEntity = new Product { Id = Guid.NewGuid(), Name = createDTO.Name, ShortDescription = createDTO.ShortDescription, FullDescription = createDTO.FullDescription, Price = createDTO.Price, DiscountPrice = createDTO.DiscountPrice, IsActive = createDTO.IsActive, IsFeatured = createDTO.IsFeatured, SKU = createDTO.SKU, StockQuantity = createDTO.StockQuantity, MinimumStockThreshold = createDTO.MinimumStockThreshold, AllowBackorder = createDTO.AllowBackorder, Brand = createDTO.Brand, Category = createDTO.Category, Tags = createDTO.Tags, ImageUrl = createDTO.ImageUrl, ThumbnailUrl = createDTO.ThumbnailUrl, SeoTitle = createDTO.SeoTitle, Slug = createDTO.Slug, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            
            _mapperMock.Setup(m => m.Map<Product>(createDTO)).Returns(mappedEntity);
            _productRepositoryMock.Setup(r => r.AddAsync(mappedEntity)).ThrowsAsync(new Exception("Repository failure."));

            // Act
            Func<Task> act = async () => await _productService.CreateAsync(createDTO);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
            _productRepositoryMock.Verify(r => r.AddAsync(mappedEntity), Times.Once);
        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoRecordsExist()
        {
            // Arrange
            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _productRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
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

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductReadDTO>>(products)).Returns(mappedDTOs);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert 
            result.Should().BeEquivalentTo(mappedDTOs);
        }

        [Fact]
        public async Task GetAllAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var expectedException = new Exception("Database connection failed.");

            _productRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            Func<Task> result = async () => await _productService.GetAllAsync();

            // Assert
            await result.Should().ThrowAsync<Exception>().WithMessage("Database connection failed.");
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            Func<Task> result = async () => await _productService.GetByIdAsync(emptyId);

            // Assert
            await result.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid product ID (Parameter 'id')");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDTO_WhenRecordExists()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var product = new Product { Id = productId, Name = "Gaming Headset", ShortDescription = "Surround sound gaming headset", FullDescription = "High-fidelity gaming headset with noise-canceling mic and RGB lighting.", Price = 199.99m, DiscountPrice = 149.99m, IsActive = true, IsFeatured = false, SKU = "GH-003", StockQuantity = 60, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "SoundBlaze", Category = "Accessories", Tags = "headset,gaming,surround", ImageUrl = "/images/products/gaming-headset.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-headset.jpg", SeoTitle = "Gaming Headset - Surround & RGB", Slug = "gaming-headset", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            var mappedDTO = new ProductReadDTO { Id = product.Id, Name = product.Name, ShortDescription = product.ShortDescription, FullDescription = product.FullDescription, Price = product.Price, DiscountPrice = product.DiscountPrice, IsActive = product.IsActive, IsFeatured = product.IsFeatured, SKU = product.SKU, StockQuantity = product.StockQuantity, MinimumStockThreshold = product.MinimumStockThreshold, AllowBackorder = product.AllowBackorder, Brand = product.Brand, Category = product.Category, Tags = product.Tags, ImageUrl = product.ImageUrl, ThumbnailUrl = product.ThumbnailUrl, SeoTitle = product.SeoTitle, Slug = product.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductReadDTO>(product)).Returns(mappedDTO);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            result.Should().BeEquivalentTo(mappedDTO);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedException = new Exception("Repository failure.");

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(productId))
                .ThrowsAsync(expectedException);

            // Act
            Func<Task> result = async () => await _productService.GetByIdAsync(productId);

            // Assert
            await result.Should().ThrowAsync<Exception>()
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
            Func<Task> actWithNull = async () => await _productService.UpdateAsync(nullDTO);
            Func<Task> actWithEmptyId = async () => await _productService.UpdateAsync(emptyIdDTO);

            // Assert
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid product update data.");
            await actWithEmptyId.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid product update data.");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var productUpdateDTO = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Gaming Monitor", ShortDescription = "High refresh rate monitor", FullDescription = "27-inch 144Hz monitor with HDR support", Price = 899.99m, DiscountPrice = 749.99m, IsActive = true, IsFeatured = false, StockQuantity = 40, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "ViewMax", Category = "Displays", Tags = "monitor,gaming,144hz", ImageUrl = "/images/products/gaming-monitor.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-monitor.jpg", SeoTitle = "Gaming Monitor - 144Hz HDR", Slug = "gaming-monitor" };
            var existingProduct = new Product { Id = productUpdateDTO.Id, Name = "Old Name", ShortDescription = "Old Desc", FullDescription = "Old Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "OldBrand", Category = "OldCategory", Tags = "old,tags", ImageUrl = "/images/old.jpg", ThumbnailUrl = "/images/thumbs/old.jpg", SeoTitle = "Old SEO", Slug = "old-slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productUpdateDTO.Id)).ReturnsAsync(existingProduct);
            _mapperMock.Setup(m => m.Map(productUpdateDTO, existingProduct));
            _productRepositoryMock.Setup(r => r.UpdateAsync(existingProduct)).ReturnsAsync(true);

            // Act
            var result = await _productService.UpdateAsync(productUpdateDTO);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var dto = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Gaming Monitor", ShortDescription = "High refresh rate monitor", FullDescription = "27-inch 144Hz monitor with HDR support", Price = 899.99m, DiscountPrice = 749.99m, IsActive = true, IsFeatured = false, StockQuantity = 40, MinimumStockThreshold = 5, AllowBackorder = false, Brand = "ViewMax", Category = "Displays", Tags = "monitor,gaming,144hz", ImageUrl = "/images/products/gaming-monitor.jpg", ThumbnailUrl = "/images/products/thumbs/gaming-monitor.jpg", SeoTitle = "Gaming Monitor - 144Hz HDR", Slug = "gaming-monitor" };

            var existingProduct = new Product { Id = dto.Id, Name = "Old Name", ShortDescription = "Old Desc", FullDescription = "Old Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "OldBrand", Category = "OldCategory", Tags = "old,tags", ImageUrl = "/images/old.jpg", ThumbnailUrl = "/images/thumbs/old.jpg", SeoTitle = "Old SEO", Slug = "old-slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var expectedException = new Exception("Repository failure");

            _productRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync(existingProduct);
            _mapperMock.Setup(m => m.Map(dto, existingProduct));
            _productRepositoryMock.Setup(r => r.UpdateAsync(existingProduct)).ThrowsAsync(expectedException);

            // Act
            Func<Task> act = async () => await _productService.UpdateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository failure");
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteAsync(productId);

            // Assert
            result.Should().BeTrue();
            _productRepositoryMock.Verify(r => r.DeleteAsync(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedException = new Exception("Repository delete failed");

            _productRepositoryMock
                .Setup(r => r.DeleteAsync(productId))
                .ThrowsAsync(expectedException);

            // Act
            Func<Task> act = async () => await _productService.DeleteAsync(productId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository delete failed");
        }

        #endregion
    }
}