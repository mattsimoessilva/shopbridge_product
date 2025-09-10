using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Models.DTOs.Product
{
    public class ProductCreateDTO
    {
        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(300)]
        public required string ShortDescription { get; set; }

        public required string FullDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue)]
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

        public required string ThumbnailURl { get; set; }

        public required string SeoTitle { get; set; }

        public required string SeoDescription { get; set; }

        public required string Slug { get; set; }
    }
}
