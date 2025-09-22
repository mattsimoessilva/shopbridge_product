using System;

namespace ProductAPI.Models.DTOs.ProductVariant
{
    public class ProductVariantReadDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public required string VariantName { get; set; }

        public required string Color { get; set; }

        public required string Size { get; set; }

        public decimal? Price { get; set; }

        public int StockQuantity { get; set; }

        public int ReservedStockQuantity { get; set; }

        public int AvailableStock => StockQuantity - ReservedStockQuantity;

        public required string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
