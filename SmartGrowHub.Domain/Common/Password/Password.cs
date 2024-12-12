using System.Collections.Immutable;

namespace SmartGrowHub.Domain.Common.Password;

public abstract record Password
{
    public static readonly Password Empty = new EmptyPassword();

    public static Fin<Password> FromPlainText(string plainText) =>
        PlainTextPassword.From(plainText)
            .Map(password => (Password)password);

    public static Fin<Password> FromHash(ImmutableArray<byte> hash) =>
        HashedPassword.From(hash)
            .Map(password => (Password)password);

    public T Match<T>(Func<PlainTextPassword, T> plainText, Func<HashedPassword, T> hash, Func<T> empty) =>
        this switch
        {
            PlainTextPassword plainTextPassword => plainText(plainTextPassword),
            HashedPassword hashedPassword => hash(hashedPassword),
            EmptyPassword => empty(),
            _ => throw new InvalidOperationException()
        };
}
