using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.DTOs;

namespace Webapia.Application.Features.Products.Interfaces;

public interface IProductService
{
    Task<PagedResultDto<ProductDto>> GetPagedAsync(int pageIndex, int pageSize);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(Guid id);
    Task UpdateDescriptionAsync(Guid id, string? newDescription);
}