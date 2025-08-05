using Eshopping.Domain.Aggregates.UserAggregate;
using Eshopping.Infrastructure;
using Eshopping.Domain.Constants;
namespace Eshopping.API.Configuration.SeedData;

public static partial class SeedData
{
    /// <summary>
    /// Ensures a default user with a specific username exists in the database
    /// If the user does not exist, it creates and saves the new user
    /// </summary>
    /// <param name="context">The database context used to query and modify user records</param>
    private static void CreateUserIfNotExist(EshoppingContext context)
    {
        // Check user and add it if not exits
        if (context.Users.FirstOrDefault(user => user.Username.ToLower().Contains(SystemConstants.CREATOR.ToLower())) is null)
        {
            AppUser user = new(SystemConstants.USER_FIRSTNAME, SystemConstants.USER_LASTNAME, SystemConstants.CREATOR)
            {
                Creator = SystemConstants.CREATOR
            };

            context.Users.Add(user);

            context.SaveChanges();
        }
    }
}