using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ProductVariant
{
    public class ProductReviewUpdateDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; }
    }
}
