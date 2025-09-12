using Xunit;
using ProductAPI.Models.Entities;
using ProductAPI.Data;
using ProductAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;

namespace ProductAPI.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private ProductAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductAppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            return new ProductAppDbContext(options);
        }

        private ProductRepository GetRepository(ProductAppDbContext context)
        {
            return new ProductRepository(context);
        }

        #region AddAsync Method.

        [Fact]
        public async Task AddAsync_ShouldAddRecordToDatabase()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Name", ShortDescription = "Test Desc", FullDescription = "Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "TestBrand", Category = "TestCategory", Tags = "test,tags", ImageUrl = "/images/test.jpg", ThumbnailUrl = "/images/thumbs/test.jpg", SeoTitle = "Test SEO", Slug = "slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            await repository.AddAsync(product);

            // Assert
            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(savedProduct);
            savedProduct.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Variants)
                .Excluding(p => p.Reviews)
                .Excluding(p => p.Id));
        }

        [Fact]
        public async Task AddAsync_ShouldPersistCorrectData()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Name", ShortDescription = "Test Desc", FullDescription = "Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "TestBrand", Category = "TestCategory", Tags = "test,tags", ImageUrl = "/images/test.jpg", ThumbnailUrl = "/images/thumbs/test.jpg", SeoTitle = "Test SEO", Slug = "slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            await repository.AddAsync(product);

            // Assert
            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(savedProduct);
            savedProduct.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews)
                       .Excluding(p => p.Id));
        }

        [Fact]
        public async Task AddAsync_ShouldHandleNullRecordGracefully()
        {

        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyNonDeletedRecords()
        {

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_IfNoRecords()
        {

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCorrectType()
        {

        }

        #endregion

        #region GetByIdAsync Method.

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord_WithMatchingId()
        {

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullIfNotFound()
        {

        }

        [Fact]
        public async Task GetByIdAsync_ShouldIncludeVariantsAndReviews()
        {
            
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_IfProductNotFound()
        {

        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {

        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfProductNotFound()
        {

        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {

        }

        [Fact]
        public async Task DeleteAsync_ShouldPersistChanges()
        {

        }

        #endregion
    }
}