using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Common.Password;

public sealed record HashedPassword : Password, DomainType<HashedPassword, ImmutableArray<byte>>
{
    private const int MinLength = 48;

    private static readonly Error InvalidHashLength = Error.New("Invalid hash length");

    private readonly ImmutableArray<byte> _value;

    private HashedPassword(ImmutableArray<byte> value) => _value = value;

    public static Fin<HashedPassword> From(ImmutableArray<byte> repr) =>
        repr.Length >= MinLength
            ? new HashedPassword(repr)
            : InvalidHashLength;

    public ImmutableArray<byte> To() => _value;
}
