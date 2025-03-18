using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Services;

public sealed class UserService(
    IUserSessionRepository sessionRepository,
    ITokensIssuer tokenService)
    : IUserService
{
    public IO<UserSession> AddNewSessionToUser(User user, CancellationToken cancellationToken) =>
        from tokens in tokenService.CreateTokens(user)
        let session = UserSession.New(user.Id, tokens)
        from _ in sessionRepository.Add(session, cancellationToken)
        select session;
}