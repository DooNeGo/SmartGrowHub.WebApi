namespace SmartGrowHub.Domain.Errors;

public sealed class UnexpectedException(string message) : ErrorException(0)
{
    public override string Message => message;

    public override int Code => 0;

    public override bool IsExceptional => false;

    public override bool IsExpected => false;

    public override Option<ErrorException> Inner => None;

    public override ErrorException Combine(ErrorException error) => Many(this, error);

    public override Error ToError() => new UnexpectedError(message);
}
