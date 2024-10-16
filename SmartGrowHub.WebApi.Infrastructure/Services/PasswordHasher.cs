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

    private static readonly UnexpectedError PasswordAlreadyHashedError = new("The password has already been hashed");
    private static readonly UnexpectedError PasswordMustNotBeEmptyError = new("The password must not be empty");
    private static readonly UnexpectedError HashedPasswordMustBeHashedError = new("The hashed password must be hashed");
    private static readonly UnexpectedError HashedPasswordMustNotBeEmptyError = new("The hashed password must not be empty");

    public Fin<Password> Hash(Password password) =>
        password.Match(
            plainText: password => FinSucc(password),
            hash: _ => PasswordAlreadyHashedError,
            empty: () => PasswordMustNotBeEmptyError)
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
                plainText: _ => HashedPasswordMustBeHashedError,
                hash: bytes => FinSucc(bytes),
                empty: () => HashedPasswordMustNotBeEmptyError)
            .Bind(hash => password.Match<Fin<bool>>(
                plainText: password => VerifyPlainText(password, hash.AsSpan()),
                hash: bytes => AreHashesEqual(bytes.AsSpan(), hash.AsSpan()),
                empty: () => PasswordMustNotBeEmptyError));

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