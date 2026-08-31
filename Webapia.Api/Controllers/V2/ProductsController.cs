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


    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100) throw new BadRequestException("Invalid pagination parameters.");

        return Ok(await _service.GetPagedAsync(page, pageSize));
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