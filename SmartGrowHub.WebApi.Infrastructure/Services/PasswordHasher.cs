using SmartGrowHub.Domain.Common;
using SmartGrowHub.WebApi.Application.Interfaces.Services;
using System.Security.Cryptography;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private static readonly HashAlgorithmName AlgorithmName = HashAlgorithmName.SHA512;
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    public Fin<Password> TryHash(Password password) =>
        password
            .Match(
                plainText: password => FinSucc(password),
                hashed: _ => Error.New("The password has already been hashed"))
            .Bind(Hash);

    private Fin<Password> Hash(string password)
    {
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.GetBytes(salt);

        Span<byte> passwordHash = stackalloc byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, passwordHash, Iterations, AlgorithmName);

        return Password.FromHashed([.. passwordHash, .. salt]);
    }

    public Fin<bool> TryVerify(Password password, Password hashedPassword) =>
        hashedPassword
            .Match(
                plainText: _ => Error.New($"{nameof(hashedPassword)} must be hashed"),
                hashed: bytes => FinSucc(bytes))
            .Map(hash => password.Match(
                plainText: password => VerifyPlainText(password, hash.AsSpan()),
                hashed: bytes => AreHashesEqual(bytes.AsSpan(), hash.AsSpan())));

    private bool VerifyPlainText(string password, ReadOnlySpan<byte> hash)
    {
        ReadOnlySpan<byte> passwordHash = hash[..HashSize];
        ReadOnlySpan<byte> salt = hash[(hash.Length - SaltSize)..];

        Span<byte> inputHash = stackalloc byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, inputHash, Iterations, AlgorithmName);

        return AreHashesEqual(passwordHash, inputHash);
    }

    private static bool AreHashesEqual(ReadOnlySpan<byte> hash1, ReadOnlySpan<byte> hash2) =>
        CryptographicOperations.FixedTimeEquals(hash1, hash2);
}