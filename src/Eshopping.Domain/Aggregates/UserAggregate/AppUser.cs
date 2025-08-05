using Eshopping.Domain.Seedwork;
using Eshopping.Domain.Constants;
using Eshopping.Domain.Aggregates.OrderAggregate;
namespace Eshopping.Domain.Aggregates.UserAggregate;

public class AppUser(string firstName, string lastName, string username) : Entity<Guid>
{
    public string Creator { get; set; } = SystemConstants.CREATOR;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string Username { get; private set; } = username;

    public IReadOnlyCollection<Order> Orders { get; private set; } = default!;

    public void SetId(Guid id) => ID = id;
}