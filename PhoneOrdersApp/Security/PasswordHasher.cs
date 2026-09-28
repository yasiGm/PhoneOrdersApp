using System;
using System.Security.Cryptography;

namespace PhoneOrdersApp.Security
{
    /// <summary>
    /// PBKDF2-based password hashing (no external dependency).
    /// Stored format: {iterations}.{salt-base64}.{hash-base64}
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;      // 128-bit salt
        private const int KeySize = 32;       // 256-bit derived key
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public static bool Verify(string password, string hashedValue)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedValue))
                return false;

            var parts = hashedValue.Split('.', 3);
            if (parts.Length != 3)
                return false; // not in the expected PBKDF2 format

            if (!int.TryParse(parts[0], out int iterations))
                return false;

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expectedKey = Convert.FromBase64String(parts[2]);

            byte[] actualKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expectedKey.Length);

            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
    }
}
