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
        public string VariantName { get; set; } // Ex: "Red - XL"

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(50)]
        public string Size { get; set; }

        [Range(0.00, double.MaxValue)]
        public decimal? AdditionalPrice { get; set; }

        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }

}