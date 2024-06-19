using Ballastlane.Blog.Application.Contracts.Services;
using System.Security.Cryptography;
using System.Text;

namespace Ballastlane.Blog.Infraestructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var providedPasswordHash = HashPassword(providedPassword);
            return hashedPassword == providedPasswordHash;
        }
    }
}
