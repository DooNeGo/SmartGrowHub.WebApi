using SmartGrowHub.Application.Services;
using SmartGrowHub.Application.UseCases.GrowHubs;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Endpoints;

internal sealed class RegisterGrowHubEndpoint
{
    public static ValueTask<IResult> RegisterGrowHub(
        RegisterGrowHubRequest request, RegisterGrowHubUseCase useCase, HttpContext context,
        IAccessTokenReader tokenReader, ILogger<GetGrowHubsEndpoint> logger, CancellationToken cancellationToken) => (
            from userId in tokenReader.GetUserId(context)
            from model in NonEmptyString.From(request.Model).ToIO()
            from _ in useCase.RegisterGrowHub(userId, model, cancellationToken)
            select _)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            _ => Ok(Result.Success()),
            error => HandleError(logger, error)));
}