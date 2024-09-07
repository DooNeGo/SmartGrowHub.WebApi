using SmartGrowHub.Domain.Exceptions;
using static Microsoft.AspNetCore.Http.Results;

namespace SmartGrowHub.WebApi.Modules;

internal static partial class InternalExceptionHandler
{
    public static IResult HandleInternalException(ILogger logger, InternalException exception)
    {
        LogInternalException(logger, exception.InnerException!.Message);
        return StatusCode(500);
    }

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogInternalException(ILogger logger, string message);
}