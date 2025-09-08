using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Product
{
    public class ProductUpdateDTO
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

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

        public string Brand { get; set; }

        public string Category { get; set; }

        public string Tags { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string Slug { get; set; }
    }
}