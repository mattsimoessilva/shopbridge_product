using AutoMapper;
using ProductApplication.Models.Entities;
using ProductApplication.Models.DTOs.ProductVariant;

namespace ProductApplication.Models.Profiles
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantReadDTO>();
            CreateMap<ProductVariantCreateDTO, ProductVariant>();
            CreateMap<ProductVariantUpdateDTO, ProductVariant>();
            CreateMap<ProductVariant, ProductVariant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}