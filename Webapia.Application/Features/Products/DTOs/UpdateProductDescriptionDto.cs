using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Webapia.Application.Features.Products.DTOs;

// Input model for partial PATCH
public record UpdateProductDescriptionDto(
    [property: JsonRequired] [MaxLength(1000)] string? Description
);