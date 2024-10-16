using SmartGrowHub.Shared.HttpErrors.Extensions;
using static Microsoft.AspNetCore.Http.Results;

namespace SmartGrowHub.WebApi.Modules;

internal static partial class ErrorHandler
{
    private static IResult HandleInternalError(ILogger logger, Error error)
    {
        LogError(logger, error.Message);

        return StatusCode(500);
    }

    public static IResult HandleError(ILogger logger, Error error)
    {
        if (error.Code is Errors.CancelledCode)
        {
            return Results.Empty;
        }

        if (!error.IsExpected || error.IsExceptional)
        {
            HandleInternalError(logger, error.ToException());
        }

        return BadRequest(error.ToDto());
    }

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogError(ILogger logger, string message);
}