using System.Collections.Concurrent;
using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Entities;
using Webapia.Infrastructure.Data.Seeds;

namespace Webapia.Infrastructure.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private static readonly ConcurrentDictionary<Guid, Product> Store = new(
        ProductSeedData.GetProducts()
            .Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImgUri = p.ImgUri,
                Description = p.Description,
                CreationTimestamp = p.CreationTimestamp
            })
            .ToDictionary(p => p.Id));

    public Task<Product?> GetByIdAsync(Guid id)
    {
        Store.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        var items = Store.Values.OrderBy(p => p.CreationTimestamp).AsEnumerable();
        return Task.FromResult(items);
    }

    public Task<PagedResultDto<Product>> GetPagedAsync(int pageIndex, int pageSize)
    {
        var ordered = Store.Values.OrderBy(p => p.CreationTimestamp).ToList();
        var items = ordered.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return Task.FromResult(new PagedResultDto<Product>
        {
            Items = items,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = ordered.Count
        });
    }

    public Task AddAsync(Product product)
    {
        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        Store[product.Id] = product;
        return Task.CompletedTask;
    }

    public void Remove(Product product)
    {
        Store.TryRemove(product.Id, out _);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
        // no-op: writes above are already applied
    }
}