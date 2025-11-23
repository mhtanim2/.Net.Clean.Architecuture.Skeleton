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
        CreateMap<Domain.Entities.Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        // Command to Entity mapping
        CreateMap<Features.Product.Commands.CreateProduct.CreateProductCommand, Domain.Entities.Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DateModified, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TODO: Add other mappings as needed
        // CreateMap<Features.Product.Commands.UpdateProduct.UpdateProductCommand, Domain.Entities.Product>()
        //     .ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
        //     .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
        //     .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}