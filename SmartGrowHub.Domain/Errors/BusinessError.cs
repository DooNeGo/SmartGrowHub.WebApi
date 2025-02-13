namespace SmartGrowHub.Domain.Errors;

public sealed record BusinessError(string Message) : Error
{
    public override string Message { get; } = Message;

    public override bool IsExceptional => false;

    public override bool IsExpected => false;

    public override ErrorException ToErrorException() => new BusinessException(Message, Code);
}