using Ballastlane.Blog.Infraestructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Infraestructure.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        [Fact]
        public void HashPassword_ReturnsDifferentHash_ForDifferentInput()
        {
            // Arrange
            var password1 = "TestPassword123!";
            var password2 = "AnotherPassword456!";

            // Act
            var hash1 = _passwordHasher.HashPassword(password1);
            var hash2 = _passwordHasher.HashPassword(password2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_ForIncorrectPassword()
        {
            // Arrange
            var correctPassword = "TestPassword123!";
            var incorrectPassword = "WrongPassword!";
            var hashedPassword = _passwordHasher.HashPassword(correctPassword);

            // Act
            var result = _passwordHasher.VerifyPassword(hashedPassword, incorrectPassword);

            // Assert
            Assert.False(result);
        }
    }
}
