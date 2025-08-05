using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.Infrastructure.EntityConfigurations.ProductConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.ID);
        builder.Property(product => product.Name).IsRequired();
        builder.HasIndex(product => product.ReferenceNumber).IsUnique();

        builder.HasOne(pc => pc.ProductCategory)
               .WithMany(product => product.Products)
               .HasForeignKey(product => product.ProductCategoryId);
    }
}