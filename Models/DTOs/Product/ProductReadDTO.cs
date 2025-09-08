using System;
using System.Collections.Generic;

namespace Models.DTOs.Product
{
    public class ProductReadDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        public string SKU { get; set; }

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

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<ProductVariantReadDto> Variants { get; set; }

        public List<ProductReviewReadDto> Reviews { get; set; }
    }
}
