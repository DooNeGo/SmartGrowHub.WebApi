using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class OtpExtensions
{
    public static OneTimePasswordDb ToDb(this OneTimePassword oneTimePassword) => new()
    {
        Id = oneTimePassword.Id,
        UserId = oneTimePassword.UserId,
        Value = oneTimePassword.Value,
        Expires = oneTimePassword.Expires,
    };
    
    public static Fin<OneTimePassword> ToDomain(this OneTimePasswordDb oneTimePassword) =>
        from value in NonEmptyString.From(oneTimePassword.Value)
        from id in Id<OneTimePassword>.From(oneTimePassword.Id)
        from userId in Id<User>.From(oneTimePassword.UserId)
        let expires = oneTimePassword.Expires
        select new OneTimePassword(id, userId, value, expires);
}