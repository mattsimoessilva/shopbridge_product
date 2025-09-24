using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ProductApplication.Models.DTOs.ProductReview;
using ProductApplication.Models.DTOs.ProductVariant;

namespace ProductApplication.Models.DTOs.Product
{
    public class ProductStockUpdateDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
