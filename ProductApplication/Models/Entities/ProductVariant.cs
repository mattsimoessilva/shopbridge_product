using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApplication.Models.Entities
{
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [Required]
        [MaxLength(100)]
        public required string VariantName { get; set; }

        [MaxLength(50)]
        public required string Color { get; set; }

        [MaxLength(50)]
        public required string Size { get; set; }

        public decimal? Price { get; set; }

        public int StockQuantity { get; set; }

        public int ReservedStockQuantity { get; set; } = 0;

        [NotMapped]
        public int AvailableStock => StockQuantity - ReservedStockQuantity;

        public required string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}