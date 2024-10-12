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

    public Fin<Password> Hash(Password password) =>
        password
            .Match(
                plainText: password => FinSucc(password),
                hash: _ => Error.New("The password has already been hashed"),
                empty: () => Error.New("The password must not be empty"))
            .Bind(HashPlainText);

    private Fin<Password> HashPlainText(string password)
    {
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.GetBytes(salt);

        Span<byte> passwordHash = stackalloc byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, passwordHash, Iterations, AlgorithmName);

        return Password.FromHash([.. passwordHash, .. salt]);
    }

    public Fin<bool> Verify(Password password, Password hashedPassword) =>
        hashedPassword
            .Match(
                plainText: _ => Error.New($"{nameof(hashedPassword)} must be hashed"),
                hash: bytes => FinSucc(bytes),
                empty: () => Error.New("The hashed password must not be empty"))
            .Bind(hash => password.Match<Fin<bool>>(
                plainText: password => VerifyPlainText(password, hash.AsSpan()),
                hash: bytes => AreHashesEqual(bytes.AsSpan(), hash.AsSpan()),
                empty: () => Error.New("The password must not be empty")));

    private bool VerifyPlainText(string password, ReadOnlySpan<byte> hash)
    {
        ReadOnlySpan<byte> passwordHash = hash[..HashSize];
        ReadOnlySpan<byte> salt = hash[^SaltSize..];

        Span<byte> inputHash = stackalloc byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password, salt, inputHash, Iterations, AlgorithmName);

        return AreHashesEqual(passwordHash, inputHash);
    }

    private static bool AreHashesEqual(ReadOnlySpan<byte> hash1, ReadOnlySpan<byte> hash2) =>
        CryptographicOperations.FixedTimeEquals(hash1, hash2);
}