using AutoMapper;
using CleanArchitectureApi.Application.Contracts.Persistence;
using MediatR;

namespace CleanArchitectureApi.Application.Features.Product.Queries.GetAllProducts;

/// <summary>
/// Handler for GetAllProductsQuery
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement filtering and pagination when needed
        // var products = await _productRepository.GetAsync();
        var products = await _productRepository.GetAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }
}