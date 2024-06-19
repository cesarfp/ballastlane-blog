using AutoFixture;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Services;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Application.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterUserAsync_ReturnsSuccess_WhenUserDoesNotExist()
        {
            // Arrange
            var fixture = new Fixture();
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var userService = new UserService(userRepositoryMock.Object, passwordHasherMock.Object);

            var request = fixture.Create<RegisterUserRequest>();
            userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                              .ReturnsAsync((User?)null);
            passwordHasherMock.Setup(hasher => hasher.HashPassword(request.Password))
                               .Returns("hashedPassword");

            // Act
            var result = await userService.RegisterUserAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var fixture = new Fixture();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object, new Mock<IPasswordHasher>().Object);

            var request = fixture.Create<RegisterUserRequest>();
            var existingUser = fixture.Create<User>();
            userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                              .ReturnsAsync(existingUser);

            // Act
            var result = await userService.RegisterUserAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User already exists.");
            userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
