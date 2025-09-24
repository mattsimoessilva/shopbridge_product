using AutoMapper;
using ProductApplication.Models.Entities;
using ProductApplication.Models.DTOs.ProductReview;

namespace ProductApplication.Models.Profiles
{
    public class ProductReviewProfile : Profile
    {
        public ProductReviewProfile()
        {
            CreateMap<ProductReview, ProductReviewReadDTO>();
            CreateMap<ProductReviewCreateDTO, ProductReview>();
            CreateMap<ProductReviewUpdateDTO, ProductReview>();
            CreateMap<ProductReview, ProductReview>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}