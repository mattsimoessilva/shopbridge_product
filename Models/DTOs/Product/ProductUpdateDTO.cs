using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Product
{
    public class ProductUpdateDTO
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

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        public int StockQuantity { get; set; }

        public int MinimumStockThreshold { get; set; }

        public bool AllowBackorder { get; set; }

        public required string Brand { get; set; }

        public required string Category { get; set; }

        public required string Tags { get; set; }

        public required string ImageUrl { get; set; }

        public required string ThumbnailUrl { get; set; }

        public required string SeoTitle { get; set; }

        public required string SeoDescription { get; set; }

        public required string Slug { get; set; }
    }
}