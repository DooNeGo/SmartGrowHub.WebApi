using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class AuthService(
    IUserService userService,
    IUserSessionService sessionService,
    IPasswordHasher passwordHasher)
    : IAuthService
{
    public Eff<LogInResponse> LogInAsync(LogInRequest request,
        CancellationToken cancellationToken) =>
        from user in userService.GetAsync(request.UserName, cancellationToken)
        from _ in VerifyPassword(user, request.Password).ToEff()
        from session in sessionService.CreateAsync(user, cancellationToken)
        select new LogInResponse(session);

    private Fin<Unit> VerifyPassword(User user, Password requestPassword) =>
        passwordHasher.TryVerify(requestPassword, user.Password)
            .Match(
                Succ: result => result
                    ? FinSucc(unit)
                    : Error.New(new ItemNotFoundException(nameof(User), None)),
                Fail: error => error);

    public Eff<RegisterResponse> RegisterAsync(RegisterRequest request,
        CancellationToken cancellationToken) =>
        passwordHasher.TryHash(request.User.Password).ToEff()
            .Map(hashedPassword => request.User with { Password = hashedPassword })
            .Bind(user => userService
                .AddAsync(user, cancellationToken)
                .Map(_ => new RegisterResponse()));

    public Eff<LogOutResponse> LogOutAsync(LogOutRequest request, CancellationToken cancellationToken) =>
        sessionService.RemoveAsync(request.Id, cancellationToken)
            .Map(_ => new LogOutResponse());
}
