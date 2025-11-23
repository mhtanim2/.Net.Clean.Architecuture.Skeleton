using CleanArchitectureApi.Application.DTOs;
using CleanArchitectureApi.Application.Features.Product.Commands.CreateProduct;
using CleanArchitectureApi.Application.Features.Product.Commands.DeleteProduct;
using CleanArchitectureApi.Application.Features.Product.Commands.UpdateProduct;
using CleanArchitectureApi.Application.Features.Product.Queries.GetAllProducts;
using CleanArchitectureApi.Application.Features.Product.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApi.Api.Controllers;

/// <summary>
/// Controller for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of products</returns>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="command">Product creation command</param>
    /// <returns>Created product ID</returns>
    [HttpPost]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<ActionResult<int>> CreateProduct(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, new { Id = productId });
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="command">Product update command</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<ActionResult> UpdateProduct(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }

    // TODO: Add additional endpoints as needed
    // [HttpGet("search")]
    // public async Task<ActionResult<List<ProductDto>>> SearchProducts([FromQuery] string searchTerm)
    // {
    //     var products = await _mediator.Send(new SearchProductsQuery { SearchTerm = searchTerm });
    //     return Ok(products);
    // }
    //
    // [HttpGet("category/{categoryId}")]
    // public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(int categoryId)
    // {
    //     var products = await _mediator.Send(new GetProductsByCategoryQuery { CategoryId = categoryId });
    //     return Ok(products);
    // }
    //
    // [HttpPost("{id}/upload-image")]
    // [Authorize(Roles = "Administrator,Manager")]
    // public async Task<ActionResult<string>> UploadProductImage(int id, IFormFile image)
    // {
    //     var command = new UploadProductImageCommand { Id = id, Image = image };
    //     var imageUrl = await _mediator.Send(command);
    //     return Ok(new { ImageUrl = imageUrl });
    // }
}