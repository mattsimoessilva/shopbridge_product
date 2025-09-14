using AutoMapper;
using ProductAPI.Models.Entities;
using ProductAPI.Models.DTOs.ProductReview;

namespace ProductAPI.Models.Profiles
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