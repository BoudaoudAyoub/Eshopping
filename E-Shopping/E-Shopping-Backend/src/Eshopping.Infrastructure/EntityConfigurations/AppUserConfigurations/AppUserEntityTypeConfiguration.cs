using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eshopping.Domain.Aggregates.UserAggregate;
namespace Eshopping.Infrastructure.EntityConfigurations.AppUserConfigurations;

public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(user => user.ID);
        builder.Property(user => user.Creator).IsRequired();
        builder.Property(user => user.LastName).IsRequired();
        builder.Property(user => user.FirstName).IsRequired();
        builder.Property(user => user.Username).IsRequired();
    }
}