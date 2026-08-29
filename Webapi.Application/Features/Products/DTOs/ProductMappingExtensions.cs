using Webapia.Domain.Entities;

namespace Webapia.Application.Features.Products.DTOs;

public static class ProductMappingExtensions
{
    // Entity -> DTO
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            product.ImgUri
        );
    }
}