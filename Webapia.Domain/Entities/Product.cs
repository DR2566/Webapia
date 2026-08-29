namespace Webapia.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public int CreationTimestamp { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImgUri { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}