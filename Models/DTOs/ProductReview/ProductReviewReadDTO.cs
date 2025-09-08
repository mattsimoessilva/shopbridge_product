using System;

namespace Models.DTOs.ProductReview
{
    public class ProductReviewReadDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public required string ReviewerName { get; set; }

        public int Rating { get; set; }

        public required string Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

