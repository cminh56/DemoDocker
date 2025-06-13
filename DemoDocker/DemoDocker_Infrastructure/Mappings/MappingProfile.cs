using AutoMapper;
using DemoDocker_Common.DTO;
using DemoDocker_Domain.Entities;

namespace DemoDocker_Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
} 