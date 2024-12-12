namespace SmartGrowHub.Domain.Common;

public readonly record struct NonNegativeInteger : DomainType<NonNegativeInteger, int>
{
    public static readonly NonNegativeInteger Zero = new(0);
    public static readonly NonNegativeInteger MaxValue = new(int.MaxValue);

    private const string ErrorMessage = "The value must not be negative";

    private readonly int _value;

    private NonNegativeInteger(int value) => _value = value;

    public static implicit operator int(NonNegativeInteger value) => value.To();
    public static explicit operator NonNegativeInteger(int value) => From(value).ThrowIfFail();

    public static Fin<NonNegativeInteger> From(int rawValue) =>
        rawValue >= 0 ? new NonNegativeInteger(rawValue)
            : FinFail<NonNegativeInteger>(Error.New(ErrorMessage));

    public int To() => _value;

    public override string ToString() => To().ToString();
}
