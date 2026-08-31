using FluentAssertions;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Domain.Entities;

namespace Webapia.Application.UnitTests.Features.Products.DTOs;

public class ProductMappingExtensionsTests
{
    [Fact]
    public void ToDto_MapsAllFieldsCorrectly()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Widget",
            Price = 19.99m,
            Description = "A useful widget",
            ImgUri = "https://example.com/widget.png",
            CreationTimestamp = 1234567890
        };

        // Act
        var dto = product.ToDto();

        // Assert
        dto.Id.Should().Be(product.Id);
        dto.Name.Should().Be(product.Name);
        dto.Price.Should().Be(product.Price);
        dto.Description.Should().Be(product.Description);
        dto.ImgUri.Should().Be(product.ImgUri);
    }

    [Fact]
    public void ToDto_WhenDescriptionIsNull_MapsNullDescription()
    {
        var product = new Product { Description = null };

        var dto = product.ToDto();

        dto.Description.Should().BeNull();
    }
}