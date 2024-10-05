using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Register;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IAuthService
{
    Eff<LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken);
    Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Eff<LogOutResponse> LogOutAsync(LogOutRequest request, CancellationToken cancellationToken);
}