using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ProductVariant
{
    public class ProductVariantCreateDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string VariantName { get; set; } // Ex: "Red - XL"

        [MaxLength(50)]
        public required string Color { get; set; }

        [MaxLength(50)]
        public required string Size { get; set; }

        [Range(0.00, double.MaxValue)]
        public decimal? AdditionalPrice { get; set; }

        public int StockQuantity { get; set; }

        public required string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }

}