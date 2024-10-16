using SmartGrowHub.Application.Services;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Auth.Endpoints;

public sealed class RegisterEndpoint
{
    public static Task<IResult> Register(IAuthService authService, RegisterRequestDto requestDto,
        ILogger<RegisterEndpoint> logger, CancellationToken cancellationToken) =>
        (from request in requestDto.TryToDomain().ToEff()
         from response in authService.Register(request, cancellationToken)
         select response)
        .RunAsync()
        .Map(fin => fin.Match(
            Succ: _ => Created(),
            Fail: error => HandleError(logger, error)));
}