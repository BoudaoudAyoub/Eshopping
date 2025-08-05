using Microsoft.EntityFrameworkCore;
using Eshopping.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Eshopping.Infrastructure.EntityConfigurations.OrderConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.ID);

        builder.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
    }
}