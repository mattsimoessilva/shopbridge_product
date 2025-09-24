using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Models.DTOs.ProductVariant;

namespace ProductApplication.Models.DTOs.ProductVariant
{
    public class ProductVariantStockUpdateDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
