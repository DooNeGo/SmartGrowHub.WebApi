using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserExtensions
{
    public static Fin<UserDb> TryToDb(this User user) =>
        from password in user.Password.Match(
            plainText: _ => Error.New("The password must be hashed before saving"),
            hash: hashed => FinSucc(hashed.To().ToArray()),
            empty: () => Error.New("The password must not be empty"))
        select new UserDb
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = password,
            EmailAddress = user.Email,
            DisplayName = user.DisplayName
        };

    public static Fin<User> TryToDomain(this UserDb user) =>
        from userName in UserName.From(user.UserName)
        from password in Password.FromHash([.. user.Password])
        from email in EmailAddress.From(user.EmailAddress)
        from displayName in NonEmptyString.From(user.DisplayName)
        select new User(new Id<User>(user.Id), userName,
            password, email, displayName);
}