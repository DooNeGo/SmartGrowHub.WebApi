using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

internal static class AccessTokenReaderExtensions
{
    public static IO<Id<User>> GetUserId(this IAccessTokenReader tokenReader, HttpContext context) =>
        from accessToken in context.GetAccessTokenOrError()
        from userId in tokenReader.GetUserId(accessToken).ToIO()
        select userId;
}