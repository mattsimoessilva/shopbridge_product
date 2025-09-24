using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductApplication.Data;
using ProductApplication.Models.DTOs.Product;
using ProductApplication.Models.Entities;
using ProductApplication.Repositories;
using System.Runtime.Intrinsics.Arm;

namespace ProductApplication.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock;

        public ProductRepositoryTests()
        {
            _mapperMock = new Mock<IMapper>();
        }

        private DatabaseContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            return new DatabaseContext(options);
        }

        private ProductRepository GetRepository(DatabaseContext context)
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
            var entity = new Product { Id = Guid.NewGuid(), Name = "Test Name", ShortDescription = "Test Desc", FullDescription = "Full Desc", Price = 100m, DiscountPrice = 90m, IsActive = true, IsFeatured = false, SKU = "GM-004", StockQuantity = 10, MinimumStockThreshold = 2, AllowBackorder = false, Brand = "TestBrand", Category = "TestCategory", Tags = "test,tags", ImageUrl = "/images/test.jpg", ThumbnailUrl = "/images/thumbs/test.jpg", SeoTitle = "Test SEO", Slug = "slug", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            var act = await repository.AddAsync(entity);

            // Assert
            act.Should().NotBeNull();
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews));

            var saved = await context.Products.FirstOrDefaultAsync(p => p.Id == entity.Id);
            Assert.NotNull(saved);
            saved.Should().BeEquivalentTo(entity, options =>
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
                .WithParameterName("entity");

            var allEntities = await context.Products.ToListAsync();
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
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entities = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", ShortDescription = "RGB backlit mechanical keyboard", FullDescription = "Durable mechanical keyboard with customizable RGB lighting and tactile switches.", Price = 249.99m, DiscountPrice = 199.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "KeyMaster", Category = "Accessories", Tags = "keyboard,mechanical,rgb", ImageUrl = "/images/entities/mechanical-keyboard.jpg", ThumbnailUrl = "/images/entities/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Tactile", Slug = "mechanical-keyboard", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() }
            };

            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeAssignableTo<IEnumerable<Product>>();
            act.Should().AllBeOfType<Product>();
        }

        #endregion

        #region GetByIdAsync Method.

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord_WithMatchingId()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Product { Id = id, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(act);
            act.Should().BeEquivalentTo(entity, options =>
                options.Excluding(p => p.Variants)
                       .Excluding(p => p.Reviews));
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

        [Fact]
        public async Task GetByIdAsync_ShouldIncludeVariantsAndReviews()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(entity);
            await context.SaveChangesAsync();

            var firstReference = new ProductVariant { Id = Guid.NewGuid(), Product = entity, ProductId = id, VariantName = "RGB Edition", Color = "Black", Size = "Medium", AdditionalPrice = 20.00m, StockQuantity = 50, ImageUrl = "/images/variants/rgb.jpg", IsActive = true };
            var secondReference = new ProductReview { Id = Guid.NewGuid(), Product = entity, ProductId = id, UserId = "user123", Rating = 5, Comment = "Great entity!", IsVerifiedPurchase = true };

            context.ProductVariants.Add(firstReference);
            context.ProductReviews.Add(secondReference);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.GetByIdAsync(entity.Id);

            // Assert
            act.Should().NotBeNull();
            act!.Variants.Should().NotBeNullOrEmpty();
            act.Reviews.Should().NotBeNullOrEmpty();

            var loadedFirstReference = act.Variants.First();
            loadedFirstReference.Should().BeEquivalentTo(firstReference, opts => opts
                .IgnoringCyclicReferences()
                .Excluding(v => v.Product) 
            );

            var loadedSecondReference = act.Reviews.First();
            loadedSecondReference.Should().BeEquivalentTo(secondReference, opts => opts
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
            var id = Guid.NewGuid();
            var entity = new Product { Id = id, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            // Act
            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeFalse();

            var allEntities = await context.Products.ToListAsync();
            allEntities.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);
            var id = Guid.NewGuid();
            var entity = new Product { Id = id, Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(entity);
            await context.SaveChangesAsync();

            // Act
            entity.Name = "New Name";
            entity.ShortDescription = "New Short Description";
            entity.FullDescription = "New Full Description";
            entity.Price = 0;
            entity.DiscountPrice = 0;
            entity.IsActive = false;
            entity.IsFeatured = false;
            entity.StockQuantity = 0;
            entity.MinimumStockThreshold = 0;
            entity.AllowBackorder = false;
            entity.Brand = "New Brand";
            entity.Category = "New Category";
            entity.Tags = "new, more_new";
            entity.ImageUrl = "/images/new_stuff/new-image.jpg";
            entity.ThumbnailUrl = "/images/new_stuff/new-thumb.jpg";
            entity.SeoTitle = "New Seo Title";
            entity.Slug = "New Slug";

            var act = await repository.UpdateAsync(entity);

            // Assert
            act.Should().BeTrue();

            var updated = await context.Products.FindAsync(id);
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
            (await context.Products.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveRecord()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();
            (await context.Products.FindAsync(entity.Id)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldPersistChanges()
        {
            // Arrange
            var context = GetDbContext();
            var repository = GetRepository(context);

            var entity = new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/entities/wireless-mouse.jpg", ThumbnailUrl = "/images/entities/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };

            context.Products.Add(entity);
            await context.SaveChangesAsync();

            // Act
            var act = await repository.DeleteAsync(entity.Id);

            // Assert
            act.Should().BeTrue();

            var existsInDb = await context.Products.AnyAsync(p => p.Id == entity.Id);
            existsInDb.Should().BeFalse();
        }

        #endregion
    }
}