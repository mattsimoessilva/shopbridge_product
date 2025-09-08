using System;

namespace Models.DTOs.ProductVariant
{
    public class ProductVariantReadDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public required string VariantName { get; set; }

        public required string Color { get; set; }

        public required string Size { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public int StockQuantity { get; set; }

        public required string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
