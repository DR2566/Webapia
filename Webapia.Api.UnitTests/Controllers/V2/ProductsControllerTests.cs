using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Webapia.Api.Controllers.V2;
using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.UnitTests.Controllers.V2;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock = new();
    private readonly ProductsController _sut;

    public ProductsControllerTests()
    {
        _sut = new ProductsController(_serviceMock.Object);
    }

    // ---------- GetPaged ----------

    [Fact]
    public async Task GetPaged_WithValidParameters_ReturnsOkWithPagedResult()
    {
        var pagedResult = new PagedResultDto<ProductDto>
        {
            Items = new[] { new ProductDto(Guid.NewGuid(), "A", 10m, null, "img") },
            PageIndex = 1,
            PageSize = 10,
            TotalCount = 1
        };
        _serviceMock.Setup(s => s.GetPagedAsync(1, 10)).ReturnsAsync(pagedResult);

        var result = await _sut.GetPaged(1, 10);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(pagedResult);
    }

    [Fact]
    public async Task GetPaged_UsesDefaultParameters_WhenNoneProvided()
    {
        var pagedResult = new PagedResultDto<ProductDto>
        {
            Items = Enumerable.Empty<ProductDto>(),
            PageIndex = 1,
            PageSize = 10,
            TotalCount = 0
        };
        _serviceMock.Setup(s => s.GetPagedAsync(1, 10)).ReturnsAsync(pagedResult);

        await _sut.GetPaged();

        _serviceMock.Verify(s => s.GetPagedAsync(1, 10), Times.Once);
    }

    [Theory]
    [InlineData(0, 10)] // page below minimum
    [InlineData(-1, 10)] // negative page
    [InlineData(1, 0)] // pageSize below minimum
    [InlineData(1, 101)] // pageSize above maximum
    public async Task GetPaged_WithInvalidParameters_ThrowsBadRequestException(int page, int pageSize)
    {
        var act = async () => await _sut.GetPaged(page, pageSize);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Invalid pagination parameters.");

        _serviceMock.Verify(
            s => s.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }

    [Theory]
    [InlineData(1, 1)] // minimum boundary
    [InlineData(1, 100)] // maximum boundary
    public async Task GetPaged_WithBoundaryValidParameters_DoesNotThrow(int page, int pageSize)
    {
        _serviceMock.Setup(s => s.GetPagedAsync(page, pageSize))
            .ReturnsAsync(new PagedResultDto<ProductDto>
            {
                Items = Enumerable.Empty<ProductDto>(),
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = 0
            });

        var act = async () => await _sut.GetPaged(page, pageSize);

        await act.Should().NotThrowAsync();
    }

    // ---------- GetById ----------

    [Fact]
    public async Task GetById_WhenProductExists_ReturnsOkWithDto()
    {
        var id = Guid.NewGuid();
        var dto = new ProductDto(id, "Widget", 9.99m, "desc", "img");
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);

        var result = await _sut.GetById(id);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(dto);
    }

    [Fact]
    public async Task GetById_WhenServiceThrowsNotFound_PropagatesException()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id))
            .ThrowsAsync(new NotFoundException("Product not found."));

        var act = async () => await _sut.GetById(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ---------- UpdateDescription ----------

    [Fact]
    public async Task UpdateDescription_WhenSuccessful_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateProductDescriptionDto("new description");

        var result = await _sut.UpdateDescription(id, dto);

        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.UpdateDescriptionAsync(id, "new description"), Times.Once);
    }
}