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
            var referenceId = Guid.NewGuid();
            var dto = new ProductReviewCreateDTO { ProductId = referenceId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var created = new ProductReviewReadDTO { ProductId = referenceId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var act = await _controller.Create(dto);

            // Assert
            var createdAct = act as CreatedAtActionResult;
            createdAct.Should().NotBeNull();
            createdAct!.Value.Should().BeEquivalentTo(created);
            createdAct.ActionName.Should().Be(nameof(_controller.GetById));
            _mockService.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        #endregion

        #region GetAll Method.

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithRecordList()
        {
            // Arrange
            var entities = new List<ProductReviewReadDTO>
            {
                new() { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true },
                new() { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Not comfortable and not responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true },
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var act = await _controller.GetAll();

            // Assert
            var okAct = act as OkObjectResult;
            okAct.Should().NotBeNull();
            okAct!.Value.Should().BeEquivalentTo(entities);
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            var referenceId = Guid.NewGuid();
            var created = new ProductReviewReadDTO { ProductId = referenceId, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = true };

            _mockService.Setup(s => s.GetByIdAsync(referenceId)).ReturnsAsync(created);

            // Act
            var act = await _controller.GetById(referenceId);

            // Assert
            var okAct = act as OkObjectResult;
            okAct.Should().NotBeNull();
            okAct!.Value.Should().BeEquivalentTo(created);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ProductReviewReadDTO?)null);

            // Act
            var act = await _controller.GetById(id);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Update Method.

        [Fact]
        public async Task Update_ShouldReturnOk_WithUpdatedRecord()
        {
            // Arrange
            var id = Guid.NewGuid();
            var referenceId = Guid.NewGuid();
            var dto = new ProductReviewUpdateDTO { Id = id, Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };
            var updated = new ProductReviewReadDTO { ProductId = referenceId, UserId = "something123", Rating = 5, Comment = dto.Comment, CreatedAt = DateTime.UtcNow.AddDays(-5), IsVerifiedPurchase = dto.IsVerifiedPurchase };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(true);

            // Act
            var act = await _controller.Update(dto);

            // Assert
            act.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenUpdateFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductReviewUpdateDTO { Id = id, Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);

            // Act
            var act = await _controller.Update(dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
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
            var act = await _controller.Delete(id);

            // Assert
            act.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

            // Act
            var act = await _controller.Delete(id);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}