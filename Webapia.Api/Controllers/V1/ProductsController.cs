using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;

namespace Webapia.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    /// <summary>
    ///     Retrieves all available products.
    /// </summary>
    /// <returns>The full list of products.</returns>
    /// <response code="200">The list of products was retrieved successfully.</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    ///     Retrieves a single product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The requested product.</returns>
    /// <response code="200">The product was found and returned.</response>
    /// <response code="404">No product exists with the given id.</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        return Ok(product);
    }

    /// <summary>
    ///     Updates the description of an existing product.
    /// </summary>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <param name="dto">The new description. Pass a null value to clear the existing description.</param>
    /// <response code="204">The description was updated successfully.</response>
    /// <response code="400">The request body is missing or malformed.</response>
    /// <response code="404">No product exists with the given id.</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpPatch("{id:guid}/description")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateProductDescriptionDto dto)
    {
        await _service.UpdateDescriptionAsync(id, dto.Description);
        return NoContent();
    }
}