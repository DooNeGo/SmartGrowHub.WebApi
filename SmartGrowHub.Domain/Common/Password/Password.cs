using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Common.Password;

public abstract record Password
{
    public static readonly Password Empty = new EmptyPassword();

    public static Fin<Password> FromPlainText(string plainText) =>
        PlainTextPassword.From(plainText)
            .Map<Password>(password => password);

    public static Fin<Password> FromHash(ImmutableArray<byte> hash) =>
        HashedPassword.From(hash)
            .Map<Password>(password => password);

    public T Match<T>(
        Func<PlainTextPassword, T> mapPlainText,
        Func<HashedPassword, T> mapHash,
        Func<T> mapEmpty) =>
        this switch
        {
            PlainTextPassword password => mapPlainText(password),
            HashedPassword password => mapHash(password),
            EmptyPassword => mapEmpty(),
            _ => throw new InvalidOperationException()
        };
}
