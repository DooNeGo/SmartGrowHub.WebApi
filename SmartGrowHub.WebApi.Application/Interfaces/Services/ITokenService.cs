using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface ITokenService
{
    Eff<AuthTokens> CreateTokens(User user);
}