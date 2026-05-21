namespace SmartGrowHub.Domain.Errors;

public class BusinessException(string message, int code) : ErrorException(code)
{
    public BusinessException(string message, int code, BusinessException inner) : this(message, code) =>
        Inner = inner;

    public override string Message { get; } = message;

    public override int Code { get; } = code;

    public override bool IsExceptional => false;

    public override bool IsExpected => true;

    public override Option<ErrorException> Inner { get; } = None;

    public override ErrorException Combine(ErrorException error) => Many(this, error);

    public override Error ToError() => new BusinessError(Message);
}