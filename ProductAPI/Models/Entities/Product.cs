using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductAPI.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(300)]
        public required string ShortDescription { get; set; }

        public required string FullDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        [Required]
        [MaxLength(50)]
        public required string SKU { get; set; }

        public int StockQuantity { get; set; }

        public int MinimumStockThreshold { get; set; }

        public bool AllowBackorder { get; set; }

        public required string Brand { get; set; }

        public required string Category { get; set; }

        public required string Tags { get; set; }

        public required string ImageUrl { get; set; }

        public required string ThumbnailUrl { get; set; }

        public required string SeoTitle { get; set; }

        public required string Slug { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties.
        public required ICollection<ProductVariant> Variants { get; set; }

        public required ICollection<ProductReview> Reviews { get; set; }

    }
}