using AutoMapper;
using ProductApplication.Models.Entities;
using ProductApplication.Models.DTOs.Product;

namespace ProductApplication.Models.Profiles
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