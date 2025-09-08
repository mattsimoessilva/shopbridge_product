using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.ProductReview
{
    public class ProductReviewCreateDTO
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReviewerName { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; } = false;
    }
}