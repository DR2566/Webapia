// tests/Webapia.Application.UnitTests/Features/Products/Services/ProductServiceTests.cs

using FluentAssertions;
using Moq;
using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Application.Features.Products.Services;
using Webapia.Domain.Entities;
using Webapia.Domain.Exceptions;

namespace Webapia.Application.UnitTests.Features.Products.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock = new();
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_repositoryMock.Object);
    }

    // ---------- GetByIdAsync ----------

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsMappedDto()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Name = "Widget", Price = 9.99m };
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

        var result = await _sut.GetByIdAsync(id);

        result.Id.Should().Be(id);
        result.Name.Should().Be("Widget");
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var act = async () => await _sut.GetByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Product not found.");
    }

    // ---------- GetAllAsync ----------

    [Fact]
    public async Task GetAllAsync_ReturnsAllProductsMappedToDtos()
    {
        var products = new[]
        {
            new Product { Id = Guid.NewGuid(), Name = "A" },
            new Product { Id = Guid.NewGuid(), Name = "B" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(p => p.Name).Should().BeEquivalentTo("A", "B");
    }

    [Fact]
    public async Task GetAllAsync_WhenNoProducts_ReturnsEmptyCollection()
    {
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Product>());

        var result = await _sut.GetAllAsync();

        result.Should().BeEmpty();
    }

    // ---------- GetPagedAsync ----------

    [Fact]
    public async Task GetPagedAsync_ReturnsMappedPagedResult_WithCorrectMetadata()
    {
        var products = new[] { new Product { Id = Guid.NewGuid(), Name = "A" } };
        var pagedResult = new PagedResultDto<Product>
        {
            Items = products,
            PageIndex = 2,
            PageSize = 10,
            TotalCount = 25
        };
        _repositoryMock.Setup(r => r.GetPagedAsync(2, 10)).ReturnsAsync(pagedResult);

        var result = await _sut.GetPagedAsync(2, 10);

        result.PageIndex.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(25);
        result.Items.Should().ContainSingle(p => p.Name == "A");
    }

    // ---------- UpdateDescriptionAsync ----------

    [Fact]
    public async Task UpdateDescriptionAsync_WhenProductExists_UpdatesDescriptionAndSaves()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Description = "old" };
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

        await _sut.UpdateDescriptionAsync(id, "new description");

        product.Description.Should().Be("new description");
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateDescriptionAsync_WhenDescriptionIsNull_SetsDescriptionToNull()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Description = "old" };
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

        await _sut.UpdateDescriptionAsync(id, null);

        product.Description.Should().BeNull();
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateDescriptionAsync_WhenProductDoesNotExist_ThrowsNotFoundException_AndDoesNotSave()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var act = async () => await _sut.UpdateDescriptionAsync(id, "new");

        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}