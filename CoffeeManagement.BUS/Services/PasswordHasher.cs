using System;
using System.Globalization;
using System.Security.Cryptography;
using CoffeeManagement.BUS.Exceptions;

namespace CoffeeManagement.BUS.Services
{
    public sealed class PasswordHasher
    {
        private const int Iterations = 210000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 10)
                throw new BusinessException("Password must contain at least 10 characters.");
            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create()) random.GetBytes(salt);
            byte[] hash;
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                hash = derive.GetBytes(HashSize);
            return "PBKDF2-SHA256$" + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public bool Verify(string password, string encoded)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(encoded)) return false;
            try
            {
                string[] parts = encoded.Split('$');
                if (parts.Length != 4 || parts[0] != "PBKDF2-SHA256") return false;
                int iterations;
                if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out iterations)) return false;
                if (iterations < 100000 || iterations > 1000000) return false;
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expected = Convert.FromBase64String(parts[3]);
                if (salt.Length < 16 || expected.Length != HashSize) return false;
                byte[] actual;
                using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                    actual = derive.GetBytes(HashSize);
                int difference = 0;
                for (int i = 0; i < expected.Length; i++) difference |= expected[i] ^ actual[i];
                return difference == 0;
            }
            catch (FormatException) { return false; }
            catch (ArgumentException) { return false; }
        }
    }
}
