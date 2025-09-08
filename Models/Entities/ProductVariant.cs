using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Models.Entities
{
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [MaxLength(100)]
        public string VariantName { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(50)]
        public string Size { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal? AdditionalPrice { get; set; }

        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}