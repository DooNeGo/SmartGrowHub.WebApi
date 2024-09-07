using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Domain.Responses;

namespace SmartGrowHub.WebApi.Application.Interfaces.Services;

public interface IAuthService
{
    EitherAsync<Exception, LogInResponse> LogInAsync(LogInRequest request, CancellationToken cancellationToken);
    EitherAsync<Exception, RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}