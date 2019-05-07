using System;
using System.Text;

namespace SFood.Auth.Common.Extensions
{
    public static class EncryptionExtension
    {
        public static string SHA256(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(src);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
