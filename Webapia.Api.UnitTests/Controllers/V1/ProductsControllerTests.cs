
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Webapia.Api.Controllers.V1;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.UnitTests.Controllers.V1;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock = new();
    private readonly ProductsController _sut;

    public ProductsControllerTests()
    {
        _sut = new ProductsController(_serviceMock.Object);
    }

    // ---------- GetAll ----------

    [Fact]
    public async Task GetAll_ReturnsOkWithAllProducts()
    {
        var products = new[]
        {
            new ProductDto(Guid.NewGuid(), "A", 10m, null, "img-a"),
            new ProductDto(Guid.NewGuid(), "B", 20m, "desc", "img-b")
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        var result = await _sut.GetAll();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetAll_WhenNoProducts_ReturnsOkWithEmptyCollection()
    {
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(Enumerable.Empty<ProductDto>());

        var result = await _sut.GetAll();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeAssignableTo<IEnumerable<ProductDto>>()
            .Which.Should().BeEmpty();
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

        // The controller doesn't catch this itself — it's the
        // ExceptionHandlingMiddleware's job. This test documents
        // and locks in that division of responsibility.
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

    [Fact]
    public async Task UpdateDescription_PassesNullDescriptionThrough()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateProductDescriptionDto(null);

        await _sut.UpdateDescription(id, dto);

        _serviceMock.Verify(s => s.UpdateDescriptionAsync(id, null), Times.Once);
    }

    [Fact]
    public async Task UpdateDescription_WhenServiceThrowsNotFound_PropagatesException()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateProductDescriptionDto("new");
        _serviceMock.Setup(s => s.UpdateDescriptionAsync(id, "new"))
            .ThrowsAsync(new NotFoundException("Product not found."));

        var act = async () => await _sut.UpdateDescription(id, dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}