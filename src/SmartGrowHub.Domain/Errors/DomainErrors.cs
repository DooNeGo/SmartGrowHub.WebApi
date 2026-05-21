namespace SmartGrowHub.Domain.Errors;

public enum DomainErrorCode
{
    RefreshTokenExpired,
    RegisterFailed,
    LogInFailed,
    SessionNotFound,
    SessionExpired,
    SettingNotFound,
    UserNotFound,
    InvalidEmailAddress
}

public static class DomainErrors
{
    public static readonly Error RefreshTokenExpiredError =
        Error.New((int)DomainErrorCode.RefreshTokenExpired, "The refresh token has already expired");

    public static readonly Error SessionNotFoundError =
        Error.New((int)DomainErrorCode.SessionNotFound, "The session was not found");

    public static readonly Error SessionExpiredError =
        Error.New((int)DomainErrorCode.SessionExpired, "The session has already expired");

    public static readonly Error SettingNotFoundError =
        Error.New((int)DomainErrorCode.SettingNotFound, "The setting was not found");

    public static readonly Error UserNotFoundError =
        Error.New((int)DomainErrorCode.UserNotFound, "The user was not found");

    public static readonly Error LogInFailedError =
        Error.New((int)DomainErrorCode.UserNotFound, "Invalid username or password");

    public static readonly Error InvalidEmailAddressError =
        Error.New((int)DomainErrorCode.InvalidEmailAddress, "Invalid email address");
}
