namespace SmartGrowHub.Domain.Errors;

public sealed record UnexpectedError : Error
{
    private readonly string _message;

    public UnexpectedError(string message) => _message = message;

    public override string Message => _message;

    public override bool IsExceptional => false;

    public override bool IsExpected => false;

    public override ErrorException ToErrorException() => new UnexpectedException(_message);
}
