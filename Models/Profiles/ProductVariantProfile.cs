using AutoMapper;

namespace Models.Profiles
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