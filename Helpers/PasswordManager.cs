using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace NetProductivity.Helpers
{
    public class PasswordManager
    {
        /// <summary>
        /// size for salt
        /// </summary>
        private const int SaltByteSize = 24;

        /// <summary>
        /// size for hash
        /// </summary>
        private const int HashByteSize = 24;

        /// <summary>
        /// the number of iterations
        /// </summary>
        private const int HashingIterationsCount = 10101;

        /// <summary>
        /// For setting safe password during registration
        /// </summary>
        /// <param name="password">users password</param>
        /// <returns>if password valid</returns>
        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{5,50}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(password))
            {
                throw new ArgumentException("Password should contain at least one lower case letter");
            }

            if (!hasUpperChar.IsMatch(password))
            {
                throw new ArgumentException("Password should contain at least one upper case letter");
            }
            if (!hasMiniMaxChars.IsMatch(password))
            {
                throw new ArgumentException("Password should not be less than 5 or greater than 50 characters");
            }
            if (!hasNumber.IsMatch(password))
            {
                throw new ArgumentException("Password should contain at least one numeric value");
            }
            if (!hasSymbols.IsMatch(password))
            {
                throw new ArgumentException("Password should contain at least one special case characters");
            }
            return true;
        }

        /// <summary>
        /// Encoded user's password
        /// </summary>
        /// <param name="password">user's password</param>
        /// <returns>hash password</returns>
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("Incorrect password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, HashingIterationsCount))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(HashByteSize);
            }

            byte[] dst = new byte[SaltByteSize + HashByteSize + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// Check hashed password
        /// </summary>
        /// <param name="hashedPassword">password from db</param>
        /// <param name="password">credential's password</param>
        /// <returns>if password correct</returns>
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] passwordHashBytes;

            int _arrayLen = SaltByteSize + HashByteSize + 1;

            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException("Incorrect password");
            }

            byte[] src = Convert.FromBase64String(hashedPassword);

            if (src.Length != _arrayLen || src[0] != 0)
            {
                return false;
            }

            byte[] currentSaltBytes = new byte[SaltByteSize];
            Buffer.BlockCopy(src, 1, currentSaltBytes, 0, SaltByteSize);

            byte[] currentHashBytes = new byte[HashByteSize];
            Buffer.BlockCopy(src, SaltByteSize + 1, currentHashBytes, 0, HashByteSize);

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, currentSaltBytes, HashingIterationsCount))
            {
                passwordHashBytes = bytes.GetBytes(SaltByteSize);
            }

            return AreHashesEqual(currentHashBytes, passwordHashBytes);

        }

        /// <summary>
        /// check hashes on equality
        /// </summary>
        /// <param name="firstHash">from db</param>
        /// <param name="secondHash">from credential</param>
        /// <returns>if they equal</returns>
        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < minHashLength; i++)
            {
                xor |= firstHash[i] ^ secondHash[i];
            }

            return xor == 0;
        }
    }
}
