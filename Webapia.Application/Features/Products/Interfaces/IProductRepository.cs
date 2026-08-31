using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Domain.Entities;

namespace Webapia.Application.Features.Products.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<PagedResultDto<Product>> GetPagedAsync(int pageIndex, int pageSize);
    Task AddAsync(Product product);
    void Remove(Product product);
    Task SaveChangesAsync();
}