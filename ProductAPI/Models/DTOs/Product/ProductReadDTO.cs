using System;
using System.Collections.Generic;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;

namespace ProductAPI.Models.DTOs.Product
{
    public class ProductReadDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string ShortDescription { get; set; }

        public required string FullDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

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

        public required string SeoDescription { get; set; }

        public required string Slug { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public required List<ProductVariantReadDTO> Variants { get; set; }

        public required List<ProductReviewReadDTO> Reviews { get; set; }
    }
}
