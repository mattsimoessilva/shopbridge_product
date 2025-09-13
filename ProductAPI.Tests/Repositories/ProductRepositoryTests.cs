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
    public class ProductRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock;

        public ProductRepositoryTests()
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

        private ProductRepository GetRepository(ProductAppDbContext context)
        {
            return new ProductRepository(context, _mapperMock.Object);
        }

        #region AddAsync Method.

        [Fact]
        public async Task AddAsync_ShouldPersistAndReturnEntity()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Name", ShortDescription = "Test Desc", FullDescription = "Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "TestBrand", Category = "TestCategory", Tags = "test,tags", ImageUrl = "/images/test.jpg", ThumbnailUrl = "/images/thumbs/test.jpg", SeoTitle = "Test SEO", Slug = "slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            var result = await repository.AddAsync(product);

            // Assert
            result.Should().NotBeNull();
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews));

            var saved = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.NotNull(saved);
            saved.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews));
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
                .WithParameterName("product");

            var allProducts = await context.Products.ToListAsync();
            allProducts.Should().BeEmpty();
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
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCorrectType()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", ShortDescription = "RGB backlit mechanical keyboard", FullDescription = "Durable mechanical keyboard with customizable RGB lighting and tactile switches.", Price = 249.99m, DiscountPrice = 199.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "KeyMaster", Category = "Accessories", Tags = "keyboard,mechanical,rgb", ImageUrl = "/images/products/mechanical-keyboard.jpg", ThumbnailUrl = "/images/products/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Tactile", Slug = "mechanical-keyboard", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() }
            };

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Product>>();
            result.Should().AllBeOfType<Product>();
        }

        #endregion

        #region GetByIdAsync Method.

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord_WithMatchingId()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var targetId = Guid.NewGuid();
            var product = new Product { Id = targetId, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(targetId);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullIfNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await repository.GetByIdAsync(nonExistentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldIncludeVariantsAndReviews()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var productId = Guid.NewGuid();
            var product = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var variant = new ProductVariant { Id = Guid.NewGuid(), Product = product, ProductId = productId, VariantName = "RGB Edition", Color = "Black", Size = "Medium", AdditionalPrice = 20.00m, StockQuantity = 50, ImageUrl = "/images/variants/rgb.jpg", IsActive = true };
            var review = new ProductReview { Id = Guid.NewGuid(), Product = product, ProductId = productId, UserId = "user123", Rating = 5, Comment = "Great product!", IsVerifiedPurchase = true };

            context.ProductVariants.Add(variant);
            context.ProductReviews.Add(review);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Variants.Should().NotBeNullOrEmpty();
            result.Reviews.Should().NotBeNullOrEmpty();

            var loadedVariant = result.Variants.First();
            loadedVariant.Should().BeEquivalentTo(variant, opts => opts
                .IgnoringCyclicReferences()
                .Excluding(v => v.Product) 
            );

            var loadedReview = result.Reviews.First();
            loadedReview.Should().BeEquivalentTo(review, opts => opts
                .IgnoringCyclicReferences()
                .Excluding(r => r.Product) 
            );
        }

        #endregion

        #region UpdateAsync Method.

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_IfProductNotFound()
        {
            // Arrange 
            var context = GetDbContext();
            var repository = GetRepository(context);
            var nonExistentId = Guid.NewGuid();
            var entity = new Product { Id = nonExistentId, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            var result = await repository.UpdateAsync(entity);

            // Assert
            result.Should().BeFalse();

            var allProducts = await context.Products.ToListAsync();
            allProducts.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var productId = Guid.NewGuid();
            var originalProduct = new Product { Id = productId, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(originalProduct);
            await context.SaveChangesAsync();

            // Act
            originalProduct.Name = "New Name";
            originalProduct.ShortDescription = "New Short Description";
            originalProduct.FullDescription = "New Full Description";
            originalProduct.Price = 0;
            originalProduct.DiscountPrice = 0;
            originalProduct.IsActive = false;
            originalProduct.IsFeatured = false;
            originalProduct.StockQuantity = 0;
            originalProduct.MinimumStockThreshold = 0;
            originalProduct.AllowBackorder = false;
            originalProduct.Brand = "New Brand";
            originalProduct.Category = "New Category";
            originalProduct.Tags = "new, more_new";
            originalProduct.ImageUrl = "/images/new_stuff/new-image.jpg";
            originalProduct.ThumbnailUrl = "/images/new_stuff/new-thumb.jpg";
            originalProduct.SeoTitle = "New Seo Title";
            originalProduct.Slug = "New Slug";

            var result = await repository.UpdateAsync(originalProduct);

            // Assert
            result.Should().BeTrue();

            var updated = await context.Products.FindAsync(productId);
            updated.Should().BeEquivalentTo(originalProduct);
        }

        #endregion

        #region DeleteAsync Method.

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_IfProductNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var nonexistentId = Guid.NewGuid();

            // Act
            var result = await repository.DeleteAsync(nonexistentId);

            // Assert
            result.Should().BeFalse();
            (await context.Products.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteAsync(product.Id);

            // Assert
            result.Should().BeTrue();
            (await context.Products.FindAsync(product.Id)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteAsync(product.Id);

            // Assert
            result.Should().BeTrue();

            var existsInDb = await context.Products.AnyAsync(p => p.Id == product.Id);
            existsInDb.Should().BeFalse();
        }

        #endregion
    }
}