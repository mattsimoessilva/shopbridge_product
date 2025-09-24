using System.ComponentModel.DataAnnotations;

namespace ProductApplication.Models.DTOs.ProductReview
{
    public class ProductReviewUpdateDTO
    {
        [Required]
        public required Guid Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public required string Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; }
    }
}
