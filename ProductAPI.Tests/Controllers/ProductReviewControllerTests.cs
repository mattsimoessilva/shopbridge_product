using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.Entities;
using ProductAPI.Services.Interfaces;


namespace ProductAPI.Tests.Controllers
{
    public class ProductReviewControllerTests
    {
        private readonly Mock<IProductReviewService> _mockService;
        private readonly ProductReviewController _controller;

        public ProductReviewControllerTests()
        {
            _mockService = new Mock<IProductReviewService>();
            _controller = new ProductReviewController(_mockService.Object);
        }

        #region Create Method.

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRecordIsCreated()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var dto = new ProductReviewCreateDTO { ProductId = productId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var created = new ProductReviewReadDTO { ProductId = productId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.Value.Should().BeEquivalentTo(created);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            _mockService.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        #endregion

        #region GetAll Method.

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithRecordList()
        {
            // Arrange
            var reviews = new List<ProductReviewReadDTO>
            {
                new() { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true },
                new() { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Not comfortable and not responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true },
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(reviews);
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var created = new ProductReviewReadDTO { ProductId = productId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true };

            _mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(created);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(created);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ProductReviewReadDTO?)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Update Method.

        [Fact]
        public async Task Update_ShouldReturnOk_WithUpdatedRecord()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var dto = new ProductReviewUpdateDTO { Id = id, Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var updated = new ProductReviewReadDTO { ProductId = productId, UserId = "something123", Rating = 5, Comment = dto.Comment, CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = dto.IsVerifiedPurchase };

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
            var id = Guid.NewGuid();
            var dto = new ProductReviewUpdateDTO { Id = id, Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

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
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}