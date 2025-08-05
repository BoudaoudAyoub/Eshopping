using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eshopping.Domain.Aggregates.ProductAggregate;
namespace Eshopping.Infrastructure.EntityConfigurations.ProductConfigurations;

public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(pc => pc.ID);
        builder.Property(pc => pc.Name).IsRequired();

        //Add other config if needed
    }
}