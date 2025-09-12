using AutoMapper;
using ProductAPI.Models.Entities;
using ProductAPI.Models.DTOs.ProductVariant;

namespace ProductAPI.Models.Profiles
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantReadDTO>();
            CreateMap<ProductVariantCreateDTO, ProductVariant>();
            CreateMap<ProductVariantUpdateDTO, ProductVariant>();
        }
    }
}