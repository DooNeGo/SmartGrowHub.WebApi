﻿using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Model.Extensions;

internal static class UserExtensions
{
    public static Fin<UserDb> TryToDb(this User user) =>
        from rawPassword in user.Password.Match<Fin<byte[]>>(
            plainText: _ => Error.New("The password must be hashed before saving"),
            hashed: bytes => bytes.ToArray(),
            empty: () => Error.New("The password must not be empty"))
        select new UserDb(user.Id, user.UserName, rawPassword, user.Email, user.DisplayName);

    public static Fin<User> TryToDomain(this UserDb user) =>
        from userName in UserName.From(user.UserName)
        from password in Password.FromHash([.. user.Password])
        from email in EmailAddress.From(user.Email)
        from displayName in NonEmptyString.From(user.DisplayName)
        select new User(new Id<User>(user.Id), userName, password, email, displayName);
}