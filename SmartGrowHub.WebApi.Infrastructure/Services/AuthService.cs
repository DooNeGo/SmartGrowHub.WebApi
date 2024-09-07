using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Exceptions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;
using SmartGrowHub.WebApi.Application.Interfaces.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class AuthService(
    IUserService userService,
    ITokenService tokenService,
    IPasswordHasher passwordHasher)
    : IAuthService
{
    public EitherAsync<Exception, LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken) =>
        userService.GetAsync(request.UserName, cancellationToken)
            .Bind<User>(user => passwordHasher
                .Verify(request.Password, user.Password)
                    ? user
                    : new ItemNotFoundException(nameof(User), None))
            .Map(user => new LogInResponse(user.Id, tokenService.CreateToken(user)));

    public EitherAsync<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken) =>
        Id(request.User)
            .Map(user => user with { Password = (Password)passwordHasher.GetHash(user.Password) })
            .Map(user => userService
                .AddAsync(user, cancellationToken)
                .Map(_ => new RegisterResponse()))
            .Value;
}
