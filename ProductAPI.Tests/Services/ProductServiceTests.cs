using AutoMapper;
using FluentAssertions;
using Moq;
using ProductAPI.Models.DTOs.Product;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;
using ProductAPI.Models.Entities;
using ProductAPI.Repositories.Interfaces;
using ProductAPI.Services;

namespace ProductAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
        }

        #region Testing CreateAsync Method

        [Fact]
        public async Task CreateAsyncMethod_ShouldThrowArgumentNullException_WhenDTOisNull()
        {
            // Arrange
            ProductCreateDTO createDTO = null;

            // Act
            Func<Task> act = async () => await _productService.CreateAsync(createDTO);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task CreateAsyncMethod_ShouldReturnDTO_WhenDTOIsValid()
        {
            // Arrange
            var createDTO = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var entity = new Product { Id = Guid.NewGuid(), Name = createDTO.Name, ShortDescription = createDTO.ShortDescription, FullDescription = createDTO.FullDescription, Price = createDTO.Price, DiscountPrice = createDTO.DiscountPrice, IsActive = createDTO.IsActive, IsFeatured = createDTO.IsFeatured, SKU = createDTO.SKU, StockQuantity = createDTO.StockQuantity, MinimumStockThreshold = createDTO.MinimumStockThreshold, AllowBackorder = createDTO.AllowBackorder, Brand = createDTO.Brand, Category = createDTO.Category, Tags = createDTO.Tags, ImageUrl = createDTO.ImageUrl, ThumbnailUrl = createDTO.ThumbnailUrl, SeoTitle = createDTO.SeoTitle, Slug = createDTO.Slug, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            var readDTO = new ProductReadDTO { Id = Guid.NewGuid(), Name = entity.Name, ShortDescription = entity.ShortDescription, FullDescription = entity.FullDescription, Price = entity.Price, DiscountPrice = entity.DiscountPrice, IsActive = entity.IsActive, IsFeatured = entity.IsFeatured, SKU = entity.SKU, StockQuantity = entity.StockQuantity, MinimumStockThreshold = entity.MinimumStockThreshold, AllowBackorder = entity.AllowBackorder, Brand = entity.Brand, Category = entity.Category, Tags = entity.Tags, ImageUrl = entity.ImageUrl, ThumbnailUrl = entity.ThumbnailUrl, SeoTitle = entity.SeoTitle, Slug = entity.Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() };

            _mapperMock.Setup(m => m.Map<Product>(createDTO)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ProductReadDTO>(entity)).Returns(readDTO);
            _productRepositoryMock.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _productService.CreateAsync(createDTO);

            // Assert
            result.Should().BeEquivalentTo(readDTO);

            _productRepositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
            _mapperMock.Verify(m => m.Map<Product>(createDTO), Times.Once);
            _mapperMock.Verify(m => m.Map<ProductReadDTO>(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsyncMethod_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var createDTO = new ProductCreateDTO { Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse" };
            var mappedEntity = new Product { Id = Guid.NewGuid(), Name = createDTO.Name, ShortDescription = createDTO.ShortDescription, FullDescription = createDTO.FullDescription, Price = createDTO.Price, DiscountPrice = createDTO.DiscountPrice, IsActive = createDTO.IsActive, IsFeatured = createDTO.IsFeatured, SKU = createDTO.SKU, StockQuantity = createDTO.StockQuantity, MinimumStockThreshold = createDTO.MinimumStockThreshold, AllowBackorder = createDTO.AllowBackorder, Brand = createDTO.Brand, Category = createDTO.Category, Tags = createDTO.Tags, ImageUrl = createDTO.ImageUrl, ThumbnailUrl = createDTO.ThumbnailUrl, SeoTitle = createDTO.SeoTitle, Slug = createDTO.Slug, Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() };
            
            _mapperMock.Setup(m => m.Map<Product>(createDTO)).Returns(mappedEntity);
            _productRepositoryMock.Setup(r => r.AddAsync(mappedEntity)).ThrowsAsync(new Exception("Repository failure."));

            // Act
            Func<Task> act = async () => await _productService.CreateAsync(createDTO);

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
            _productRepositoryMock.Verify(r => r.AddAsync(mappedEntity), Times.Once);
        }

        #endregion

        #region Testing GetAllAsync Method

        [Fact]
        public async Task GetAllAsyncMethod_ShouldReturnEmptyList_WhenNoRecordsExist()
        {
            // Arrange
            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _productRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsyncMethod_ShouldReturnMappedDTOs_WhenRecordsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", ShortDescription = "Ergonomic wireless mouse", FullDescription = "Comfortable wireless mouse with adjustable DPI and silent clicks.", Price = 129.99m, DiscountPrice = 99.99m, IsActive = true, IsFeatured = true, SKU = "WM-001", StockQuantity = 150, MinimumStockThreshold = 10, AllowBackorder = false, Brand = "LogiTech", Category = "Accessories", Tags = "mouse,wireless,ergonomic", ImageUrl = "/images/products/wireless-mouse.jpg", ThumbnailUrl = "/images/products/thumbs/wireless-mouse.jpg", SeoTitle = "Wireless Mouse - Ergonomic & Silent", Slug = "wireless-mouse", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() },
                new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", ShortDescription = "RGB backlit mechanical keyboard", FullDescription = "Durable mechanical keyboard with customizable RGB lighting and tactile switches.", Price = 249.99m, DiscountPrice = 199.99m, IsActive = true, IsFeatured = false, SKU = "MK-002", StockQuantity = 80, MinimumStockThreshold = 5, AllowBackorder = true, Brand = "KeyMaster", Category = "Accessories", Tags = "keyboard,mechanical,rgb", ImageUrl = "/images/products/mechanical-keyboard.jpg", ThumbnailUrl = "/images/products/thumbs/mechanical-keyboard.jpg", SeoTitle = "Mechanical Keyboard - RGB & Tactile", Slug = "mechanical-keyboard", Variants = new List<ProductVariant>(), Reviews = new List<ProductReview>() }
            };

            var mappedDTOs = new List<ProductReadDTO>
            {
                new ProductReadDTO { Id = products[0].Id, Name = products[0].Name, ShortDescription = products[0].ShortDescription, FullDescription = products[0].FullDescription, Price = products[0].Price, DiscountPrice = products[0].DiscountPrice, IsActive = products[0].IsActive, IsFeatured = products[0].IsFeatured, SKU = products[0].SKU, StockQuantity = products[0].StockQuantity, MinimumStockThreshold = products[0].MinimumStockThreshold, AllowBackorder = products[0].AllowBackorder, Brand = products[0].Brand, Category = products[0].Category, Tags = products[0].Tags, ImageUrl = products[0].ImageUrl, ThumbnailUrl = products[0].ThumbnailUrl, SeoTitle = products[0].SeoTitle, Slug = products[0].Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() },
                new ProductReadDTO { Id = products[1].Id, Name = products[1].Name, ShortDescription = products[1].ShortDescription, FullDescription = products[1].FullDescription, Price = products[1].Price, DiscountPrice = products[1].DiscountPrice, IsActive = products[1].IsActive, IsFeatured = products[1].IsFeatured, SKU = products[1].SKU, StockQuantity = products[1].StockQuantity, MinimumStockThreshold = products[1].MinimumStockThreshold, AllowBackorder = products[1].AllowBackorder, Brand = products[1].Brand, Category = products[1].Category, Tags = products[1].Tags, ImageUrl = products[1].ImageUrl, ThumbnailUrl = products[1].ThumbnailUrl, SeoTitle = products[1].SeoTitle, Slug = products[1].Slug, Variants = new List<ProductVariantReadDTO>(), Reviews = new List<ProductReviewReadDTO>() }
            };

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductReadDTO>>(products)).Returns(mappedDTOs);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert 
        }

        [Fact]
        public async Task GetAllAsyncMethod_ShouldThrowException_WhenRepositoryFails()
        {
        }

        #endregion

        #region Testing GetById Method

        [Fact]
        public async Task GetByIdAsyncMethod_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
        }

        [Fact]
        public async Task GetByIdAsyncMethod_ShouldReturnMappedDTO_WhenRecordExists()
        {
        }

        [Fact]
        public async Task GetByIdAsyncMethod_ShouldThrowException_WhenRepositoryFails()
        {
        }

        #endregion

        #region Testing UpdateAsync Method

        [Fact]
        public async Task UpdateAsyncMethod_ShouldThrowArgumentException_WhenDTOisNullOrIdIsEmpty()
        {
        }

        [Fact]
        public async Task UpdateAsyncMethod_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
        }

        [Fact]
        public async Task UpdateAsyncMethod_ShouldThrowException_WhenRepositoryFails()
        {
        }

        #endregion

        #region Testing DeleteAsync Method

        [Fact]
        public async Task DeleteAsyncMethod_ShouldCallRepository_WithCorrectId()
        {
        }

        [Fact]
        public async Task DeleteAsyncMethod_ShouldThrowException_WhenRepositoryFails()
        {
        }

        #endregion
    }
}