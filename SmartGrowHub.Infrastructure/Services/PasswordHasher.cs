using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Domain.Errors;
using System.Security.Cryptography;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private static readonly HashAlgorithmName AlgorithmName = HashAlgorithmName.SHA512;
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    private static readonly UnexpectedError PasswordMustNotBeEmptyError = new("The password must not be empty");
    private static readonly UnexpectedError HashedPasswordMustBeHashedError = new("The hashed password must be hashed");
    private static readonly UnexpectedError HashedPasswordMustNotBeEmptyError = new("The hashed password must not be empty");

    public Fin<HashedPassword> Hash(PlainTextPassword password)
    {
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.GetBytes(salt);

        Span<byte> passwordHash = stackalloc byte[HashSize];
        Rfc2898DeriveBytes.Pbkdf2(password.To(), salt, passwordHash, Iterations, AlgorithmName);

        return HashedPassword.From([.. passwordHash, .. salt]);
    }

    public Fin<bool> Verify(Password password1, Password password2) =>
        password2
            .Match(
                plainText: _ => HashedPasswordMustBeHashedError,
                hash: bytes => FinSucc(bytes.To()),
                empty: () => HashedPasswordMustNotBeEmptyError)
            .Bind(hash => password1.Match<Fin<bool>>(
                plainText: password => VerifyPlainText(password, hash.AsSpan()),
                hash: bytes => AreHashesEqual(bytes.To().AsSpan(), hash.AsSpan()),
                empty: () => PasswordMustNotBeEmptyError));

    private static bool VerifyPlainText(string password, ReadOnlySpan<byte> hash)
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