using Microsoft.EntityFrameworkCore;
using Webapia.Application.Common;
using Webapia.Application.Common.Pagination;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Entities;
using Webapia.Infrastructure.Data;

namespace Webapia.Infrastructure.Repositories;

public class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public EfProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.CreationTimestamp)
            .ToListAsync();
    }

    public async Task<PagedResultDto<Product>> GetPagedAsync(int pageIndex, int pageSize)
    {
        var totalCount = await _context.Products.CountAsync();
        var items = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.CreationTimestamp)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<Product>
            { Items = items, PageIndex = pageIndex, PageSize = pageSize, TotalCount = totalCount };
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}