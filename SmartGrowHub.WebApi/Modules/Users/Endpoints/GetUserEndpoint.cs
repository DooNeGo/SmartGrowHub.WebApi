using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.WebApi.Modules.Extensions;
using SmartGrowHub.WebApi.Modules.Users.Extensions;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.WebApi.Modules.ErrorHandler;

namespace SmartGrowHub.WebApi.Modules.Users.Endpoints;

public sealed class GetUserEndpoint
{
    public static ValueTask<IResult> GetUser(
        IUserRepository userRepository, IAccessTokenReader tokenReader,
        HttpContext context, ILogger<GetUserEndpoint> logger,
        CancellationToken cancellationToken) => (
            from accessToken in context
                .GetAccessToken()
                .ReduceTransformer(Error.New("There is no access token in the headers"))
            from userId in tokenReader.GetUserId(accessToken).ToIO()
            from user in userRepository
                .GetById(userId, cancellationToken)
                .ReduceTransformer(DomainErrors.UserNotFoundError)
            select user)
        .RunSafeAsync()
        .Map(fin => fin.Match(
            Succ: user => Ok(user.ToDto()),
            Fail: error => HandleError(logger, error)));
}