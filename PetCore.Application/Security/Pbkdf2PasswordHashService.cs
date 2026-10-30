using System.Security.Cryptography;
using System.Text;
using PetCore.Application.Security;

namespace PetCore.Application.Security;

public sealed class Pbkdf2PasswordHashService : IPasswordHashService
{
    private const string Prefix = "pbkdf2-sha256";
    private const int Iterations = 310_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Prefix}${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string storedValue)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedValue)) return false;
        if (!IsHash(storedValue))
        {
            if (storedValue.StartsWith(Prefix + "$", StringComparison.Ordinal)) return false;
            var supplied = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var legacy = SHA256.HashData(Encoding.UTF8.GetBytes(storedValue));
            return CryptographicOperations.FixedTimeEquals(supplied, legacy);
        }

        var parts = storedValue.Split('$');
        if (parts.Length != 4 || !int.TryParse(parts[1], out var iterations) || iterations is < 100_000 or > 1_000_000)
            return false;
        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
            if (salt.Length < SaltSize || expected.Length != HashSize) return false;
            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public bool IsHash(string storedValue) => storedValue.StartsWith(Prefix + "$", StringComparison.Ordinal);
}