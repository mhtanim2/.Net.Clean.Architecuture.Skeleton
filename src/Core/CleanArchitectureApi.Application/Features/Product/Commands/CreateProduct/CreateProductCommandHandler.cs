using AutoMapper;
using CleanArchitectureApi.Application.Contracts.Persistence;
using CleanArchitectureApi.Application.Exceptions;
using CleanArchitectureApi.Domain.Entities;
using MediatR;

namespace CleanArchitectureApi.Application.Features.Product.Commands.CreateProduct;

/// <summary>
/// Handler for CreateProductCommand
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate incoming data
        var validator = new CreateProductCommandValidator(_productRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException("Invalid Product", validationResult);

        // Convert to domain entity
        var product = _mapper.Map<Domain.Entities.Product>(request);
        product.CreatedAt = DateTime.UtcNow;

        // Add to database
        await _productRepository.CreateAsync(product);

        // Return record id
        return product.Id;
    }
}