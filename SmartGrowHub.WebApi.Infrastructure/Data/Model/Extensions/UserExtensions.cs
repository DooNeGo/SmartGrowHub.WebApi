using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserExtensions
{
    public static UserDb ToDb(this User user) =>
        new(user.Id, user.UserName, user.Password,
            user.Email, user.DisplayName, []);

    public static Fin<User> TryToDomain(this UserDb user) =>
        User.Create(new Id<User>(user.Id), user.UserName,
            user.Password, user.Email, user.DisplayName);
}