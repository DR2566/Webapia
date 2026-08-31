using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webapia.Domain.Entities;
using Webapia.Infrastructure.Data.Seeds;

namespace Webapia.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Id)
            .HasColumnType("uniqueidentifier")
            .HasDefaultValueSql("NEWSEQUENTIALID()")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.CreationTimestamp)
            .HasColumnType("int")
            .HasDefaultValueSql("DATEDIFF(SECOND, '1970-01-01', SYSUTCDATETIME())")
            .ValueGeneratedOnAdd();

        builder.HasIndex(p => p.CreationTimestamp);

        // Property Constraints
        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p => p.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.ImgUri)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.HasData(ProductSeedData.GetProducts());
    }
}