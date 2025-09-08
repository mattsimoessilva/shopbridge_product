using System;

namespace Models.DTOs.ProductReview
{
    public class ProductReviewReadDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ReviewerName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

