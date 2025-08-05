using Eshopping.Domain.Aggregates.UserAggregate;
namespace Eshopping.Infrastructure.Repositories;
public class AppUserRepository(EshoppingContext eshoppingContext) : Repository<AppUser, Guid>(eshoppingContext), IAppUserRepository
{
}