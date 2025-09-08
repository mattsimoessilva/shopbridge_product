using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnonations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        [Required]
        [MaxLength(50)]
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

        public string Slug { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties.
        public ICollection<ProductVariant> Variants { get; set; }

        public ICollection<ProductReview> Reviews { get; set; }

    }
}