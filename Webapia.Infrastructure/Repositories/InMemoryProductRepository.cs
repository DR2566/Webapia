using System.Collections.Concurrent;
using Webapia.Application.Common.Pagination.DTOs;
using Webapia.Application.Features.Products.Interfaces;
using Webapia.Domain.Entities;

namespace Webapia.Infrastructure.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private static readonly ConcurrentDictionary<Guid, Product> Store = new(
        new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("01a04c51-6d80-732b-a63a-425edc702774"),
                Name = "Monitor",
                Price = 299.99m,
                ImgUri = "https://img.alza.cz/m1.jpg",
                Description = "27-inch 4K Display",
                CreationTimestamp = 1750000000
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-7168-7a2f-801d-56bcc4917746"),
                Name = "Keyboard",
                Price = 89.99m,
                ImgUri = "https://img.alza.cz/k1.jpg",
                Description = "Mechanical Keyboard",
                CreationTimestamp = 1750000010
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-7550-7e53-afc7-a1c969e6176c"),
                Name = "Gaming Mouse",
                Price = 59.99m,
                ImgUri = "https://img.alza.cz/mouse1.jpg",
                Description = "Wireless RGB Gaming Mouse",
                CreationTimestamp = 1750000020
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-7938-7b3b-9494-b2dfb5f11928"),
                Name = "Laptop",
                Price = 1299.99m,
                ImgUri = "https://img.alza.cz/laptop1.jpg",
                Description = "15-inch Performance Laptop",
                CreationTimestamp = 1750000030
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-7d20-7910-bb53-ca101a408cec"),
                Name = "Headphones",
                Price = 149.99m,
                ImgUri = "https://img.alza.cz/headphones1.jpg",
                Description = "Wireless Noise Cancelling Headphones",
                CreationTimestamp = 1750000040
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-8108-76b8-b0b7-bfc4e5259d1d"),
                Name = "Webcam",
                Price = 79.99m,
                ImgUri = "https://img.alza.cz/webcam1.jpg",
                Description = "Full HD USB Webcam",
                CreationTimestamp = 1750000050
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-84f0-7822-b7a7-5d3bbb201c28"),
                Name = "USB-C Hub",
                Price = 49.99m,
                ImgUri = "https://img.alza.cz/hub1.jpg",
                Description = "7-in-1 USB-C Hub",
                CreationTimestamp = 1750000060
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-88d8-7634-b067-d1e0935b1005"),
                Name = "External SSD",
                Price = 119.99m,
                ImgUri = "https://img.alza.cz/ssd1.jpg",
                Description = "1TB Portable SSD",
                CreationTimestamp = 1750000070
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-8cc0-7d77-8198-ec1cd446c538"),
                Name = "Gaming Chair",
                Price = 249.99m,
                ImgUri = "https://img.alza.cz/chair1.jpg",
                Description = "Ergonomic Gaming Chair",
                CreationTimestamp = 1750000080
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-90a8-7b30-b95b-1948e4692bcb"),
                Name = "Desk Lamp",
                Price = 39.99m,
                ImgUri = "https://img.alza.cz/lamp1.jpg",
                Description = "LED Desk Lamp",
                CreationTimestamp = 1750000090
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-9490-757c-953c-ad77c495bc14"),
                Name = "Smartphone",
                Price = 899.99m,
                ImgUri = "https://img.alza.cz/phone1.jpg",
                Description = "128GB 5G Smartphone",
                CreationTimestamp = 1750000100
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-9878-71bc-9148-22ad0dc5df84"),
                Name = "Tablet",
                Price = 599.99m,
                ImgUri = "https://img.alza.cz/tablet1.jpg",
                Description = "11-inch Tablet",
                CreationTimestamp = 1750000110
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-9c60-7e4d-ac92-d1a700cedc55"),
                Name = "Smart Watch",
                Price = 229.99m,
                ImgUri = "https://img.alza.cz/watch1.jpg",
                Description = "Fitness and Health Tracking",
                CreationTimestamp = 1750000120
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-a048-7f61-8b5a-d4ab0b923845"),
                Name = "Bluetooth Speaker",
                Price = 99.99m,
                ImgUri = "https://img.alza.cz/speaker1.jpg",
                Description = "Portable Bluetooth Speaker",
                CreationTimestamp = 1750000130
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-a430-7f0f-b622-c2be8e9947e9"),
                Name = "Microphone",
                Price = 129.99m,
                ImgUri = "https://img.alza.cz/microphone1.jpg",
                Description = "USB Condenser Microphone",
                CreationTimestamp = 1750000140
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-a818-7a75-8c77-8f7d76aafcda"),
                Name = "Graphics Card",
                Price = 699.99m,
                ImgUri = "https://img.alza.cz/gpu1.jpg",
                Description = "High Performance Graphics Card",
                CreationTimestamp = 1750000150
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-ac00-7f69-b711-a64c0c7638b1"),
                Name = "Processor",
                Price = 349.99m,
                ImgUri = "https://img.alza.cz/cpu1.jpg",
                Description = "8-Core Desktop Processor",
                CreationTimestamp = 1750000160
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-afe8-7451-baa4-d7240366440d"),
                Name = "RAM",
                Price = 159.99m,
                ImgUri = "https://img.alza.cz/ram1.jpg",
                Description = "32GB DDR5 Memory Kit",
                CreationTimestamp = 1750000170
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-b3d0-7ec4-8c9c-7ae4b141ae6c"),
                Name = "Motherboard",
                Price = 219.99m,
                ImgUri = "https://img.alza.cz/motherboard1.jpg",
                Description = "ATX Gaming Motherboard",
                CreationTimestamp = 1750000180
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-b7b8-70c7-ace9-1781ae65ec84"),
                Name = "Power Supply",
                Price = 109.99m,
                ImgUri = "https://img.alza.cz/psu1.jpg",
                Description = "750W Modular Power Supply",
                CreationTimestamp = 1750000190
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-bba0-7fc4-b146-9ac145fab374"),
                Name = "PC Case",
                Price = 94.99m,
                ImgUri = "https://img.alza.cz/case1.jpg",
                Description = "Tempered Glass ATX Case",
                CreationTimestamp = 1750000200
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-bf88-7671-b161-d56ed2220741"),
                Name = "Router",
                Price = 139.99m,
                ImgUri = "https://img.alza.cz/router1.jpg",
                Description = "Wi-Fi 6 Router",
                CreationTimestamp = 1750000210
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-c370-708e-9be1-e7922d4e09c1"),
                Name = "Printer",
                Price = 189.99m,
                ImgUri = "https://img.alza.cz/printer1.jpg",
                Description = "Wireless Color Printer",
                CreationTimestamp = 1750000220
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-c758-7c36-b501-267d1c97ba9b"),
                Name = "Projector",
                Price = 499.99m,
                ImgUri = "https://img.alza.cz/projector1.jpg",
                Description = "Full HD Home Projector",
                CreationTimestamp = 1750000230
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-cb40-7338-839e-307fab222384"),
                Name = "VR Headset",
                Price = 449.99m,
                ImgUri = "https://img.alza.cz/vr1.jpg",
                Description = "Virtual Reality Headset",
                CreationTimestamp = 1750000240
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-cf28-7889-ad48-b74da2898f9f"),
                Name = "Game Controller",
                Price = 69.99m,
                ImgUri = "https://img.alza.cz/controller1.jpg",
                Description = "Wireless Game Controller",
                CreationTimestamp = 1750000250
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-d310-713e-bd2f-6951020f5515"),
                Name = "Mechanical Keypad",
                Price = 49.99m,
                ImgUri = "https://img.alza.cz/keypad1.jpg",
                Description = "Programmable Mechanical Keypad",
                CreationTimestamp = 1750000260
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-d6f8-7a30-85a3-692e8858e7ff"),
                Name = "Laptop Stand",
                Price = 34.99m,
                ImgUri = "https://img.alza.cz/stand1.jpg",
                Description = "Adjustable Aluminum Laptop Stand",
                CreationTimestamp = 1750000270
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-dae0-7411-9749-67c73d2cec66"),
                Name = "Wireless Charger",
                Price = 29.99m,
                ImgUri = "https://img.alza.cz/charger1.jpg",
                Description = "Fast Wireless Charging Pad",
                CreationTimestamp = 1750000280
            },
            new Product
            {
                Id = Guid.Parse("01a04c51-dec8-77da-92fe-f30057578d28"),
                Name = "USB Flash Drive",
                Price = 19.99m,
                ImgUri = "https://img.alza.cz/usb1.jpg",
                Description = "128GB USB 3.2 Flash Drive",
                CreationTimestamp = 1750000290
            }
        }.ToDictionary(p => p.Id));

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

    public void Remove(Product product) => Store.TryRemove(product.Id, out _);

    public Task SaveChangesAsync() => Task.CompletedTask; // no-op: writes above are already applied
}