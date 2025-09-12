using AutoMapper;
using ProductAPI.Models.Entities;
using ProductAPI.Models.DTOs.Product;

namespace ProductAPI.Models.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadDTO>();
            CreateMap<ProductCreateDTO, ProductReadDTO>();
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();
            CreateMap<Product, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}