using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using ProductAPI.Models.DTOs.ProductReview;
using ProductAPI.Models.DTOs.ProductVariant;

namespace ProductAPI.Models.DTOs.Product
{
    public class ProductStockUpdateDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
