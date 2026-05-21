using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Users;

namespace SmartGrowHub.AspNetCore.Modules.Users.Extensions;

public static class UserExtensions
{
    public static UserDto ToDto(this User user) =>
        new(user.Email.ValueUnsafe()?.To(), user.PhoneNumber.ValueUnsafe()?.To());
}