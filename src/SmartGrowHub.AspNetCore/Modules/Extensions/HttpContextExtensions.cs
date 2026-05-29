using Microsoft.AspNetCore.Authentication;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

internal static class HttpContextExtensions
{
    private static readonly UnexpectedError NoTokenError =
        new("There is no access token in the headers");
    
    public static OptionT<IO, AccessToken> GetAccessToken(this HttpContext context) =>
        from rawToken in OptionT.liftIO<IO, string>(
            IO.liftAsync(() => context.GetTokenAsync("access_token").Map(Optional)))
        from accessToken in AccessToken.From(rawToken).ToIO()
        select accessToken;

    public static IO<AccessToken> GetAccessTokenOrError(this HttpContext context) =>
        from rawToken in IO
            .liftAsync(() => context.GetTokenAsync("access_token").Map(Optional))
            .Bind(option => option.Match(IO.pure, IO.fail<string>(NoTokenError)))
        from accessToken in AccessToken.From(rawToken).ToIO()
        select accessToken;
}