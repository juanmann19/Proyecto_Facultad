using System;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto_Facultad.Helpers
{
    public static class PasswordHasher
    {
        public static (string hash, string salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                var salt = Convert.ToBase64String(saltBytes);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
                {
                    var hashBytes = pbkdf2.GetBytes(20);
                    var hash = Convert.ToBase64String(hashBytes);
                    return (hash, salt);
                }
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                var hashBytes = pbkdf2.GetBytes(20);
                var hash = Convert.ToBase64String(hashBytes);
                return hash == storedHash;
            }
        }
    }
}
