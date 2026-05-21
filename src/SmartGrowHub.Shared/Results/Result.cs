namespace SmartGrowHub.Shared.Results;

public record Result(
    bool IsSuccess,
    string? ErrorMessage,
    int? ErrorCode)
{
    public static Result Success() => new(true, null, null);

    public static Result Fail(string message, int code) => new(false, message, code);

    public static Result<T> Success<T>(T data) => new(data, true, null, null);

    public static Result<T> Failure<T>(string message, int code) => new(default, false, message, code);
}

public sealed record Result<T>(
    T? Data,
    bool IsSuccess,
    string? ErrorMessage,
    int? ErrorCode)
{
    public static Result<T> Success(T data) => new(data, true, null, null);

    public static Result<T> Failure(string message, int code) => new(default, false, message, code);

    public Result<TNew> Map<TNew>(Func<T, TNew> map) =>
        Data is null
            ? new Result<TNew>(default, IsSuccess, ErrorMessage, ErrorCode)
            : new Result<TNew>(map(Data), IsSuccess, ErrorMessage, ErrorCode);
}