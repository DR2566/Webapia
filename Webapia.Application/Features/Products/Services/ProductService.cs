using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Exceptions;

namespace Webapia.Application.Features.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<ProductDto>> GetPagedAsync(int pageIndex, int pageSize)
    {
        var result = await _repository.GetPagedAsync(pageIndex, pageSize);

        return new PagedResultDto<ProductDto>
        {
            Items = result.Items.Select(p => p.ToDto()),
            PageIndex = result.PageIndex,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => p.ToDto());
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found.");

        return product.ToDto();
    }

    public async Task UpdateDescriptionAsync(Guid id, string? newDescription)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            throw new NotFoundException("Product not found.");

        product.Description = newDescription;
        await _repository.SaveChangesAsync();
    }
}