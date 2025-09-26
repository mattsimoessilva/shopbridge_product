using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApplication.Controllers;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Services.Interfaces;


namespace ProductApplication.Tests.Controllers
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
            var createdResult = act as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.Value.Should().BeEquivalentTo(created);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            _mockService.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var dto = new ProductReviewCreateDTO { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Invalid", IsVerifiedPurchase = true };
            _controller.ModelState.AddModelError("UserId", "Required");

            // Act
            var act = await _controller.Create(dto);

            // Assert
            act.Should().BeOfType<BadRequestObjectResult>();
            _mockService.Verify(s => s.CreateAsync(It.IsAny<ProductReviewCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenServiceThrowsArgumentNullException()
        {
            // Arrange
            var dto = new ProductReviewCreateDTO { ProductId = Guid.NewGuid(), UserId = "something123", Rating = 5, Comment = "Failure", IsVerifiedPurchase = true };
            _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new ArgumentNullException("dto"));

            // Act
            var act = await _controller.Create(dto);

            // Assert
            var badRequest = act as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = "dto cannot be null" });
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
                new() { ProductId = Guid.NewGuid(), UserId = "user456", Rating = 3, Comment = "Average quality", CreatedAt = DateTime.UtcNow.AddDays(-2), IsVerifiedPurchase = false }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var act = await _controller.GetAll();

            // Assert
            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithEmptyList_WhenNoRecordsExist()
        {
            // Arrange
            var entities = new List<ProductReviewReadDTO>();
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var act = await _controller.GetAll();

            // Assert
            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(entities);
            ((IEnumerable<ProductReviewReadDTO>)okResult.Value!).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Database failure"));

            // Act
            var act = await _controller.GetAll();

            // Assert
            var errorResult = act as ObjectResult;
            errorResult.Should().NotBeNull();
            errorResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            errorResult.Value.Should().BeEquivalentTo(new { error = "Database failure" });
        }

        #endregion

        #region GetById Method.

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenRecordExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new ProductReviewReadDTO { ProductId = id, UserId = "something123", Rating = 5, Comment = "Excellent", CreatedAt = DateTime.UtcNow.AddDays(-1), IsVerifiedPurchase = true };

            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(entity);

            // Act
            var act = await _controller.GetById(id);

            // Assert
            var okResult = act as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(entity);
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

        [Fact]
        public async Task GetById_ShouldReturnInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("Unexpected failure"));

            // Act
            var act = await _controller.GetById(id);

            // Assert
            var errorResult = act as ObjectResult;
            errorResult.Should().NotBeNull();
            errorResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            errorResult.Value.Should().BeEquivalentTo(new { error = "Unexpected failure" });
        }

        #endregion

        #region Update Method.

        [Fact]
        public async Task Update_ShouldReturnOk_WhenRecordIsUpdated()
        {
            // Arrange
            var dto = new ProductReviewUpdateDTO { Id = Guid.NewGuid(), Rating = 4, Comment = "Improved after usage", IsVerifiedPurchase = true };
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
            var dto = new ProductReviewUpdateDTO { Id = Guid.NewGuid(), Rating = 2, Comment = "Not great", IsVerifiedPurchase = false };
            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);

            // Act
            var act = await _controller.Update(dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenServiceThrowsArgumentException()
        {
            // Arrange
            var dto = new ProductReviewUpdateDTO { Id = Guid.NewGuid(), Rating = 1, Comment = "Invalid data", IsVerifiedPurchase = false };
            _mockService.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new ArgumentException("Invalid update data.", "dto"));

            // Act
            var act = await _controller.Update(dto);

            // Assert
            var badRequest = act as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = "dto is invalid." });
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

        [Fact]
        public async Task Delete_ShouldReturnInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(id)).ThrowsAsync(new Exception("Unexpected failure"));

            // Act
            var act = await _controller.Delete(id);

            // Assert
            var errorResult = act as ObjectResult;
            errorResult.Should().NotBeNull();
            errorResult!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            errorResult.Value.Should().BeEquivalentTo(new { error = "Unexpected failure" });
        }

        #endregion

    }
}