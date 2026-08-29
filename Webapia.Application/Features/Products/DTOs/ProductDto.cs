namespace Webapia.Application.Features.Products.DTOs;

// Output model for GET
public record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string? Description,
    string ImgUri
);