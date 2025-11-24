using AutoMapper;
using CleanArchitectureApi.Application.DTOs;
using CleanArchitectureApi.Domain.Entities;

namespace CleanArchitectureApi.Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for Product mappings
/// </summary>
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Domain.Entities.Product, ProductDto>().ReverseMap();

        // Command to Entity mapping
        CreateMap<Features.Product.Commands.CreateProduct.CreateProductCommand, Domain.Entities.Product>();

        // TODO: Add other mappings as needed
        // CreateMap<Features.Product.Commands.UpdateProduct.UpdateProductCommand, Domain.Entities.Product>();
    }
}