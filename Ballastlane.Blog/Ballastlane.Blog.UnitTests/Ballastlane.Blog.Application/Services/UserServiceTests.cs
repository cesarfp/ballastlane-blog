using AutoFixture;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Services;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Application.Services
{
    public class UserServiceTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(_userRepositoryMock.Object);

        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsSuccess_WhenUserDoesNotExist()
        {
            // Arrange
           
            var request = _fixture.Create<RegisterUserRequest>();
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                              .ReturnsAsync((User?)null);
           

            // Act
            var result = await _userService.RegisterUserAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var request = _fixture.Create<RegisterUserRequest>();
            var existingUser = _fixture.Create<User>();
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
                              .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.RegisterUserAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("User already exists.");
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateUserCredentialsAsync(user.Email, user.Password);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateUserCredentialsAsync(user.Email, "wrongpassword");

            // Assert
            result.Should().BeNull();
        }
    }
}
