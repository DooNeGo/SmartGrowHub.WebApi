using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class AuthService(
    IUserService userService,
    IPasswordHasher passwordHasher)
    : IAuthService
{
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken) =>
        from user in userService.GetByUserName(request.UserName, cancellationToken)
        from _ in VerifyPassword(user, request.Password).ToEff()
        from result in userService.AddNewSessionToUser(user, cancellationToken)
        select new LogInResponse(result.Session);

    private Fin<Unit> VerifyPassword(User user, Password requestPassword) =>
        passwordHasher
            .Verify(requestPassword, user.Password)
            .Bind(result => result
                ? FinSucc(unit)
                : Error.New(new ItemNotFoundException(nameof(User), None)));

    public Eff<RegisterResponse> Register(RegisterRequest request,
        CancellationToken cancellationToken) =>
        from _ in userService.AddNewUser(User.New(
            request.UserName, request.Password,
            request.EmailAddress, request.DisplayName),
            cancellationToken)
        select new RegisterResponse();

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken) =>
        from _ in userService.RemoveSession(request.Id, cancellationToken)
        select new LogOutResponse();

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request,
        CancellationToken cancellationToken) =>
        from newTokens in userService.RefreshTokens(request.RefreshToken, cancellationToken)
        select new RefreshTokensResponse(newTokens);
}