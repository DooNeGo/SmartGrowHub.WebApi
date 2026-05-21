using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface ITokensIssuer
{
    IO<AuthTokens> CreateTokens(User user);
}