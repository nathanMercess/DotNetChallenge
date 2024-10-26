using DotNetChallenge.Services.Contracts.Services;
using System.Security.Cryptography;

namespace DotNetChallenge.Services.Implementations.Services;

public sealed class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    private const int MaxHashLength = 60;

    public string HashPassword(string password)
    {
        using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
        {
            var salt = algorithm.Salt;
            var hash = algorithm.GetBytes(KeySize);

            var hashBytes = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes).Substring(0, MaxHashLength);
        }
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var fullHash = hashedPassword.PadRight((int)(Math.Ceiling(hashedPassword.Length / 4.0) * 4), '=');
        var hashBytes = Convert.FromBase64String(fullHash);

        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using (var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            var hash = algorithm.GetBytes(KeySize);
            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
        }

        return true;
    }
}
