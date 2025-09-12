using System.ComponentModel.DataAnnotations;
using ProductAPI.Models.Entities;


namespace ProductAPI.Tests.Models.Entities
{
    public class ProductTests
    {
        private Product CreateValidObject() => new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            ShortDescription = "Short description",
            FullDescription = "Full description",
            Price = 99.99m,
            SKU = "SKU123",
            Brand = "BrandName",
            Category = "CategoryName",
            Tags = "tag1,tag2",
            ImageUrl = "http://image.com",
            ThumbnailUrl = "http://thumb.com",
            SeoTitle = "SEO Title",
            Slug = "test-product",
            Variants = new List<ProductVariant>(),
            Reviews = new List<ProductReview>()
        };

        [Fact]
        public void Model_ShouldFailValidation_WhenNameIsMissing()
        {
            // Arrange
            var product = CreateValidObject();
            product.Name = null!;

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void Model_ShouldFailValidation_WhenSKUExceedsMaxLength()
        {
            // Arrange
            var product = CreateValidObject();
            product.SKU = new string('X', 51);

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(product, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("SKU"));
        }

        [Fact]
        public void Model_DefaultValues_ShouldBeCorrect()
        {
            // Arrange
            var product = CreateValidObject();

            // Assert
            Assert.True(product.IsActive);
            Assert.False(product.IsFeatured);
            Assert.True(product.CreatedAt <= DateTime.UtcNow);
        }
    }
}