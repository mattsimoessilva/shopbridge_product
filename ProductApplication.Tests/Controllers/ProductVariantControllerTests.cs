using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApplication.Controllers;
using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.DTOs.ProductVariant;
using ProductApplication.Services.Interfaces;


namespace ProductApplication.Tests.Controllers
{
    public class ProductVariantControllerTests
    {
        private readonly Mock<IProductVariantService> _mockService;
        private readonly ProductVariantController _controller;

        public ProductVariantControllerTests()
        {
            _mockService = new Mock<IProductVariantService>();
            _controller = new ProductVariantController(_mockService.Object);
        }

        #region Create Method.

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenRecordIsCreated()
        {
            // Arrange
            var referenceId = Guid.NewGuid();
            var dto = new ProductVariantCreateDTO { ProductId = referenceId, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var created = new ProductVariantReadDTO { Id = Guid.NewGuid(), ProductId = referenceId, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

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
            var referenceId = Guid.NewGuid();

            var entities = new List<ProductVariantReadDTO>
            {
                new() { Id = Guid.NewGuid(), ProductId = referenceId, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true },
                new() { Id = Guid.NewGuid(), ProductId = referenceId, VariantName = "Hel Edition", Color = "White", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-white.jpg", IsActive = true }
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
            var created = new ProductVariantReadDTO { Id = Guid.NewGuid(), ProductId = referenceId, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

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
            _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ProductVariantReadDTO?)null);

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
            var dto = new ProductVariantUpdateDTO { Id = id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };
            var updated = new ProductVariantReadDTO { Id = id, VariantName = dto.VariantName, Color = dto.Color, Size = dto.Size, Price = dto.Price, StockQuantity = dto.StockQuantity, ImageUrl = dto.ImageUrl, IsActive = dto.IsActive };

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
            var dto = new ProductVariantUpdateDTO { Id = id, VariantName = "Blackout Edition", Color = "Black", Size = "Standard", Price = 0, StockQuantity = 70, ImageUrl = "/images/products/gaming-headset-black.jpg", IsActive = true };

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

        #region ReserveStock Method.

        [Fact]
        public async Task ReserveStock_ShouldReturnOk_WhenReservationSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 5 };
            _mockService.Setup(s => s.ReserveStockAsync(id, dto.Quantity)).ReturnsAsync(true);

            // Act
            var act = await _controller.ReserveStock(id, dto);

            // Assert
            act.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ReserveStock_ShouldReturnNotFound_WhenReservationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 5 };
            _mockService.Setup(s => s.ReserveStockAsync(id, dto.Quantity)).ReturnsAsync(false);

            // Act
            var act = await _controller.ReserveStock(id, dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReserveStock_ShouldReturnBadRequest_WhenInvalidOperationExceptionThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 5 };
            var errorMessage = "Insufficient stock.";
            _mockService.Setup(s => s.ReserveStockAsync(id, dto.Quantity))
                        .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            var act = await _controller.ReserveStock(id, dto);

            // Assert
            var badRequest = act as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = errorMessage });
        }

        #endregion

        #region ReleaseStock Method.

        [Fact]
        public async Task ReleaseStock_ShouldReturnOk_WhenReleaseSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 3 };
            _mockService.Setup(s => s.ReleaseStockAsync(id, dto.Quantity)).ReturnsAsync(true);

            // Act
            var act = await _controller.ReleaseStock(id, dto);

            // Assert
            act.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ReleaseStock_ShouldReturnNotFound_WhenReleaseFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 3 };
            _mockService.Setup(s => s.ReleaseStockAsync(id, dto.Quantity)).ReturnsAsync(false);

            // Act
            var act = await _controller.ReleaseStock(id, dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReleaseStock_ShouldReturnBadRequest_WhenInvalidOperationExceptionThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductStockUpdateDTO { Quantity = 3 };
            var errorMessage = "Stock was not reserved.";
            _mockService.Setup(s => s.ReleaseStockAsync(id, dto.Quantity))
                        .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            var act = await _controller.ReleaseStock(id, dto);

            // Assert
            var badRequest = act as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = errorMessage });
        }

        #endregion

        #region ReduceStock Method.

        [Fact]
        public async Task ReduceStock_ShouldReturnOk_WhenReductionSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductVariantStockUpdateDTO { Quantity = 2 };
            _mockService.Setup(s => s.ReduceStockAsync(id, dto.Quantity)).ReturnsAsync(true);

            // Act
            var act = await _controller.ReduceStock(id, dto);

            // Assert
            act.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ReduceStock_ShouldReturnNotFound_WhenReductionFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductVariantStockUpdateDTO { Quantity = 2 };
            _mockService.Setup(s => s.ReduceStockAsync(id, dto.Quantity)).ReturnsAsync(false);

            // Act
            var act = await _controller.ReduceStock(id, dto);

            // Assert
            act.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReduceStock_ShouldReturnBadRequest_WhenInvalidOperationExceptionThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new ProductVariantStockUpdateDTO { Quantity = 2 };
            var errorMessage = "Cannot reduce below zero.";
            _mockService.Setup(s => s.ReduceStockAsync(id, dto.Quantity))
                        .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            var act = await _controller.ReduceStock(id, dto);

            // Assert
            var badRequest = act as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.Value.Should().BeEquivalentTo(new { error = errorMessage });
        }

        #endregion
    }
}