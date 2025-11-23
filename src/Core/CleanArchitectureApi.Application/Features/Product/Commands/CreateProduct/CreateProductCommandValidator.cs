using CleanArchitectureApi.Application.Contracts.Persistence;
using FluentValidation;

namespace CleanArchitectureApi.Application.Features.Product.Commands.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to 0");

        RuleFor(p => p.SKU)
            .MustAsync(IsUniqueSKU).WithMessage("SKU must be unique")
            .When(p => !string.IsNullOrEmpty(p.SKU));

        // TODO: Add custom validation rules as needed
        // RuleFor(p => p.Description)
        //     .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters");
    }

    private async Task<bool> IsUniqueSKU(string sku, CancellationToken cancellationToken)
    {
        // TODO: Implement SKU uniqueness check when repository method is available
        // var products = await _productRepository.GetBySKUAsync(sku);
        // return products == null;
        return true;
    }
}