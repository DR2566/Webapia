namespace Webapia.Application.Features.Products.DTOs;

// Input model for partial PATCH
public record UpdateProductDescriptionDto(
    string? Description
);