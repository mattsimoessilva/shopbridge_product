using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Services.Interfaces;
using Xunit;

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

        [Fact]
        public async Task CreateShouldReturnCreatedAtActionWhenProductIsCreated()
        {
            // Arrange
            var dto = new ProductCreateDTO { Name = "Wireless Mouse", Price = 129.9m };
            var created = new ProductReadDTO { Id = Guid.NewGuid(), Name = dto.Name, Price = dto.Price };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.Value.Should().BeEquivalentTo(created);
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
        }
    }
}