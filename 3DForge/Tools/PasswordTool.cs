﻿using System.Security.Cryptography;

namespace Backend3DForge.Tools
{
    public static class PasswordTool
    {
        public const int SaltByteSize = 32;
        public const int HashByteSize = 480;
        public const int HashingIterationsCount = 10210;

        public static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltByteSize]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashingIterationsCount);
            byte[] hash = pbkdf2.GetBytes(HashByteSize);
            byte[] hashBytes = new byte[HashByteSize + SaltByteSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltByteSize);
            Array.Copy(hash, 0, hashBytes, SaltByteSize, HashByteSize);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool Validate(string password, string passwordHash)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(passwordHash);

                byte[] salt = new byte[SaltByteSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltByteSize);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashingIterationsCount);
                byte[] hash = pbkdf2.GetBytes(HashByteSize);

                for (int i = 0; i < HashByteSize; i++)
                    if (hashBytes[i + SaltByteSize] != hash[i])
                        return false;
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
