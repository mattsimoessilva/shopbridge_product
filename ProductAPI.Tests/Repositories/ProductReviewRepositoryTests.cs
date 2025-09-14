using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductAPI.Data;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories;
using System.Runtime.Intrinsics.Arm;

namespace ProductAPI.Tests.Repositories
{
    public class ProductReviewRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock;

        public ProductReviewRepositoryTests()
        {
            _mapperMock = new Mock<IMapper>();
        }

        private ProductAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductAppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            return new ProductAppDbContext(options);
        }

        private ProductReviewRepository GetRepository(ProductAppDbContext context)
        {
            return new ProductReviewRepository(context, _mapperMock.Object);
        }

        #region AddAsync Method.

        [Fact]
        public async Task AddAsync_ShouldPersistAndReturnEntity()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);
            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            // Act
            var act = await repository.AddAsync(entity);

            // Assert
            act.Should().NotBeNull();
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity);

            var saved = await context.ProductReviews.FirstOrDefaultAsync(p => p.Id == entity.Id);
            Assert.NotNull(saved);
            saved.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task AddAsync_ShouldHandleNullRecordGracefully()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            // Act
            Func<Task> act = () => repository.AddAsync(null);

            // Assert
            await act
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName("entity");

            var allEntities = await context.ProductReviews.ToListAsync();
            allEntities.Should().BeEmpty();
        }

        #endregion

        #region GetAllAsync Method.

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_IfNoRecords()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            // Act
            var act = await repository.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCorrectType()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);

            var entities = new List<ProductReview>
            {
                new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true },
                new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "somethingelse123", Rating = 1, Comment = "Not comfortable and not responsive!", IsVerifiedPurchase = true }
            };

            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeAssignableTo<IEnumerable<ProductReview>>();
            act.Should().AllBeOfType<ProductReview>();
        }

        #endregion

        #region GetByIdAsync Method.

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord_WithMatchingId()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new ProductReview { Id = id, Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            context.ProductReviews.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity, opts => opts
                .Excluding(x => x.Product)          
            );
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var id = Guid.NewGuid();

            // Act
            var act = await repository.GetByIdAsync(id);

            // Assert
            act.Should().BeNull();
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_IfProductNotFound()
        {
            // Arrange 
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            // Act
            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeFalse();

            var allEntities = await context.ProductReviews.ToListAsync();
            allEntities.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new ProductReview { Id = id, Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            context.ProductReviews.Add(entity);
            await context.SaveChangesAsync();

            // Act
            entity.Rating = 1;
            entity.Comment = "Not comfortable and not responsive!";
            entity.IsVerifiedPurchase = false;

            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeTrue();

            var updated = await context.ProductReviews.FindAsync(id);
            updated.Should().BeEquivalentTo(entity);
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfProductNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var id = Guid.NewGuid();

            // Act
            var act = await repository.DeleteAsync(id);

            // Assert
            act.Should().BeFalse();
            (await context.ProductReviews.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            context.ProductReviews.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();
            (await context.ProductReviews.FindAsync(entity.Id)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldPersistChanges()
        {
            // Arrange
            var reference = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new ProductReview { Id = Guid.NewGuid(), Product = reference, ProductId = reference.Id, UserId = "something123", Rating = 5, Comment = "Super comfortable and responsive!", IsVerifiedPurchase = true };

            context.ProductReviews.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();

            var existsInDb = await context.ProductReviews.AnyAsync(p => p.Id == entity.Id);
            existsInDb.Should().BeFalse();
        }

        #endregion
    }
}