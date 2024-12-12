using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface ITokensIssuer
{
    Eff<AuthTokens> CreateTokens(User user);
}