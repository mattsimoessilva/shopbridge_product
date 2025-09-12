using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;
using ProductAPI.Services.Interfaces;


namespace ProductAPI.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
        }

        #region Create Method.

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRecordIsCreated()
        {
            // Arrange
            var dto = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var created = new ProductReadDTO { Id = Guid.NewGuid(), Name = dto.Name, ShortDescription = dto.ShortDescription, FullDescription = dto.FullDescription, Price = dto.Price, DiscountPrice = dto.DiscountPrice, IsActive = dto.IsActive, IsFeatured = dto.IsFeatured, SKU = dto.SKU, StockQuantity = dto.StockQuantity, MinimumStockThreshold = dto.MinimumStockThreshold, AllowBackorder = dto.AllowBackorder, Brand = dto.Brand, Category = dto.Category, Tags = dto.Tags, ImageUrl = dto.ImageUrl, ThumbnailUrl = dto.ThumbnailUrl, SeoTitle = dto.SeoTitle, Slug = dto.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.Value.Should().BeEquivalentTo(created);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
        }

        #endregion

        #region GetAll Method.

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithRecordList()
        {
            // Arrange
            var products = new List<ProductReadDTO>
            {
                new() { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() },
                new() { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", ShortDescription = "RGB mechanical keyboard", FullDescription = "High-performance mechanical keyboard with customizable RGB lighting.", Price = 349.99m, DiscountPrice = 299.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "Corsair", Category = "Accessories", Tags = "keyboard,mechanical,RGB", ImageUrl = "/images/products/mechanical-keyboard.jpg", ThumbnailUrl = "/images/products/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Performance", Slug = "mechanical-keyboard", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(products);
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new ProductReadDTO { Id = productId, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", CreatedAt = DateTime.UtcNow, UpdatedAt = null, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync((ProductReadDTO?)null);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Update Method.

        [Fact]
        public async Task Update_ShouldReturnOk_WithUpdatedRecord()
        {
            // Arrange
            var dto = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var updated = new ProductReadDTO { Id = dto.Id, Name = dto.Name, ShortDescription = dto.ShortDescription, FullDescription = dto.FullDescription, Price = dto.Price, DiscountPrice = dto.DiscountPrice, IsActive = dto.IsActive, IsFeatured = dto.IsFeatured, SKU = "WM-001", StockQuantity = dto.StockQuantity, MinimumStockThreshold = dto.MinimumStockThreshold, AllowBackorder = dto.AllowBackorder, Brand = dto.Brand, Category = dto.Category, Tags = dto.Tags, ImageUrl = dto.ImageUrl, ThumbnailUrl = dto.ThumbnailUrl, SeoTitle = dto.SeoTitle, Slug = dto.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(dto);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenUpdateFails()
        {
            // Arrange
            var dto = new ProductUpdateDTO { Id = Guid.NewGuid(), Name = "Nonexistent Product", ShortDescription = "Nonexistent.", FullDescription = "Nonexistent.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/nonexistent.jpg", ThumbnailUrl = "/images/products/thumbs/nonexistent.jpg", SeoTitle = "Nonexistent - Ergonomic & Silent", Slug = "non-existent" };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(dto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Delete Method.

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenRecordIsDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(productId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(productId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}