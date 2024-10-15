using SmartGrowHub.Domain.Exceptions;
using static Microsoft.AspNetCore.Http.Results;

namespace SmartGrowHub.WebApi.Modules;

internal static partial class ExceptionHandler
{
    private static IResult HandleInternalException(ILogger logger, Exception exception)
    {
        Exception innerException = exception.InnerException!;

        if (innerException is TaskCanceledException) return Results.Empty;

        LogInternalException(logger, innerException.Message);

        return Problem("Internal error", null, 500, "Internal error");
    }

    public static IResult HandleException(ILogger logger, Exception exception)
    {
        if (exception is InternalException internalException)
        {
            return HandleInternalException(logger, internalException);
        }

        return Problem(exception.Message, null, 400);
    }

    public static IResult HandleError(ILogger logger, Error error)
    {
        if (error.Code is Errors.CancelledCode)
        {
            return Results.Empty;
        }

        if (error.Message is "Internal error")
        {
            HandleInternalException(logger, error.ToException());
        }

        return Problem(error.Message, null, 400);
    }

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogInternalException(ILogger logger, string message);
}