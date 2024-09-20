using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.Register;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IAuthService
{
    EitherAsync<Exception, LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken);
    EitherAsync<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}