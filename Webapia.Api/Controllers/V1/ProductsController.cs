using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;

namespace Webapia.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _service.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpPatch("{id:guid}/description")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateProductDescriptionDto dto)
    {
        await _service.UpdateDescriptionAsync(id, dto.Description);
        return NoContent();
    }
}