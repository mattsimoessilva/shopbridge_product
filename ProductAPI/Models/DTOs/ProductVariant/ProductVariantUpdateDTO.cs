using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Models.DTOs.ProductVariant
{
    public class ProductVariantUpdateDTO
    {
        [Required]
        public required Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string VariantName { get; set; }

        [MaxLength(50)]
        public required string Color { get; set; }

        [MaxLength(50)]
        public required string Size { get; set; }

        [Range(0.00, double.MaxValue)]
        public decimal? AdditionalPrice { get; set; }

        public int StockQuantity { get; set; }

        public required string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}