//password hasher
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Auth.Common.Security
{
    /// <summary>
    /// Provides password hashing and verification functionality using industry-standard algorithms.
    /// Implements PBKDF2 with HMAC-SHA256 for secure password hashing.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8; // 16 bytes
        private const int HashSize = 256 / 8; // 32 bytes
        private const int Iterations = 100000; // Number of iterations for PBKDF2
        
        /// <summary>
        /// Generates a secure hash from a password with a random salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>Tuple containing the hash and salt</returns>
        public (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Generate the hash
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            ));

            return (hash, Convert.ToBase64String(salt));
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="storedHash">The stored hash to compare against</param>
        /// <param name="storedSalt">The stored salt used in the original hash</param>
        /// <returns>True if the password matches, false otherwise</returns>
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                byte[] salt = Convert.FromBase64String(storedSalt);

                // Generate hash from the provided password
                string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: Iterations,
                    numBytesRequested: HashSize
                ));

                // Compare the computed hash with the stored hash
                return computedHash == storedHash;
            }
            catch (Exception)
            {
                // If there's any error in the process, return false
                return false;
            }
        }

        /// <summary>
        /// Validates password complexity requirements
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if password meets requirements, false otherwise</returns>
        public bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (password.Length < 8) return false;

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                else if (char.IsLower(c)) hasLowerCase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
            }

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }
    }

    /// <summary>
    /// Interface for password hashing operations
    /// </summary>
    public interface IPasswordHasher
    {
        (string Hash, string Salt) HashPassword(string password);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
        bool ValidatePasswordStrength(string password);
    }
}