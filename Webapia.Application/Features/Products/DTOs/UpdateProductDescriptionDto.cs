using System.ComponentModel.DataAnnotations;

namespace Webapia.Application.Features.Products.DTOs;

// Input model for partial PATCH
public record UpdateProductDescriptionDto(
    [MaxLength(1000)] string? Description
);