using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class AuthService(
    IUserService userService,
    IUserSessionService sessionService,
    IPasswordHasher passwordHasher)
    : IAuthService
{
    public EitherAsync<Exception, LogInResponse> LogInAsync(LogInRequest request,
        CancellationToken cancellationToken) =>
        from user in userService.GetAsync(request.UserName, cancellationToken)
        from _ in VerifyPassword(user, request.Password)
        from session in sessionService.CreateAsync(user, cancellationToken)
        select new LogInResponse(session);

    private EitherAsync<Exception, User> VerifyPassword(User user, string requestPassword) =>
        passwordHasher.Verify(requestPassword, user.Password)
            ? user : new ItemNotFoundException(nameof(User), None);

    public EitherAsync<Exception, RegisterResponse> RegisterAsync(RegisterRequest request,
        CancellationToken cancellationToken) =>
        Id(request.User)
            .Map(user => user with { Password = (Password)passwordHasher.Hash(user.Password) })
            .Map(user => userService
                .AddAsync(user, cancellationToken)
                .Map(_ => new RegisterResponse()))
            .Value;
}
