using AutoMapper;
using MinhaApi.DTOs;
using MinhaApi.Entities;

namespace MinhaApi.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>();
    }
}
