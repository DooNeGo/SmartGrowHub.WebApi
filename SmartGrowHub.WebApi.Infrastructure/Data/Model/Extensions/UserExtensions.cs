using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserExtensions
{
    public static Fin<UserDb> TryToDb(this User user) =>
        from rawPassword in user.Password.Match<Fin<byte[]>>(
            plainText: _ => Error.New("A password must be hashed before saving"),
            hashed: bytes => bytes.ToArray())
        select new UserDb(user.Id, user.UserName, rawPassword, user.Email, user.DisplayName);

    public static Fin<User> TryToDomain(this UserDb user) =>
        from userName in UserName.Create(user.UserName)
        from password in Password.FromHashed([.. user.Password])
        from email in EmailAddress.Create(user.Email)
        from displayName in NonEmptyString.Create(user.DisplayName)
        select new User(new Id<User>(user.Id), userName, password, email, displayName);
}