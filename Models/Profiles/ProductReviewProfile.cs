using AutoMapper;

namespace Models.Profiles
{
    public class ProductReviewProfile : Profile
    {
        public ProductReviewProfile()
        {
            CreateMap<ProductReview, ProductReviewReadDTO>();
            CreateMap<ProductReviewCreateDTO, ProductReview>();
            CreateMap<ProductReviewUpdateDTO, ProductReview>();
        }
    }
}