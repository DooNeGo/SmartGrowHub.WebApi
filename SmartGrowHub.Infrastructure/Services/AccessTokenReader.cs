using Microsoft.IdentityModel.JsonWebTokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class AccessTokenReader : IAccessTokenReader
{
    private static readonly JsonWebTokenHandler TokenHandler = new();

    public Fin<Id<User>> GetUserId(AccessToken accessToken) =>
        Domain.Common.Id<User>
            .From(TokenHandler.ReadJsonWebToken(accessToken).Subject)
            .MapFail(error => Error.New("Invalid access token", error));
}
