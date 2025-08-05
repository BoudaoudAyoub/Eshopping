using Eshopping.Domain.Seedwork;
namespace Eshopping.Domain.Aggregates.UserAggregate;
public interface IAppUserRepository : IRepository<AppUser, Guid> {}