using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of available products.
    /// </summary>
    /// <param name="page">The page number to retrieve (1-based). Defaults to 1.</param>
    /// <param name="pageSize">The number of products per page (1-100). Defaults to 10.</param>
    /// <returns>A paged result containing the requested slice of products.</returns>
    /// <response code="200">The page of products was retrieved successfully.</response>
    /// <response code="400">The page or pageSize parameters are invalid (page &lt; 1, pageSize &lt; 1, or pageSize &gt; 100).</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // controller input validation
        if (page < 1 || pageSize < 1 || pageSize > 100) throw new BadRequestException("Invalid pagination parameters.");

        return Ok(await _service.GetPagedAsync(page, pageSize));
    }

    /// <summary>
    /// Retrieves a single product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The requested product.</returns>
    /// <response code="200">The product was found and returned.</response>
    /// <response code="404">No product exists with the given id.</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        return Ok(product);
    }

    /// <summary>
    /// Updates the description of an existing product.
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
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateProductDescriptionDto dto)
    {
        await _service.UpdateDescriptionAsync(id, dto.Description);
        return NoContent();
    }
}