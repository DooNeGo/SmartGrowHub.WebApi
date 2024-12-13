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
        from value in NonNegativeInteger.From(oneTimePassword.Value)
        let id = new Id<OneTimePassword>(oneTimePassword.Id)
        let userId = new Id<User>(oneTimePassword.UserId)
        let expires = oneTimePassword.Expires
        select new OneTimePassword(id, userId, value, expires);
}