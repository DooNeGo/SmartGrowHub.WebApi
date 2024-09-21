using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.Register;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IAuthService
{
    Eff<LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken);
    Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}