using AutoFixture;
using AutoFixture.AutoMoq;
using Ballastlane.Blog.Api.Controllers;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Dtos;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Api
{
    public class UserControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockUserService = _fixture.Freeze<Mock<IUserService>>();
            _mockTokenService = _fixture.Freeze<Mock<ITokenService>>();
            _controller = new UserController(_mockUserService.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task RegisterUser_ReturnsOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var request = _fixture.Create<RegisterUserRequest>();
            _mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<RegisterUserRequest>()))
                            .ReturnsAsync(new RegistrationResult { IsSuccess = true });

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var request = _fixture.Create<RegisterUserRequest>();
            _mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<RegisterUserRequest>()))
                            .ReturnsAsync(new RegistrationResult { IsSuccess = false, Message = "User already exists." });

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be("User already exists.");
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Email is required");
            var request = _fixture.Create<RegisterUserRequest>();

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RegisterUser_ReturnsStatusCode500_WhenExceptionOccurs()
        {
            // Arrange
            var request = _fixture.Create<RegisterUserRequest>();
            _mockUserService.Setup(s => s.RegisterUserAsync(It.IsAny<RegisterUserRequest>()))
                            .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.RegisterUser(request);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                  .Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = _fixture.Create<LoginRequest>();
            _mockUserService.Setup(s => s.ValidateUserCredentialsAsync(loginRequest.Email, loginRequest.Password))
                            .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var loginRequest = _fixture.Build<LoginRequest>()
                                       .With(x => x.Email, user.Email) // Ensure the email matches the user's email
                                       .Create();
            var expectedToken = _fixture.Create<string>();

            _mockUserService.Setup(s => s.ValidateUserCredentialsAsync(loginRequest.Email, loginRequest.Password))
                            .ReturnsAsync(user);
            _mockTokenService.Setup(s => s.GenerateToken(user))
                            .Returns(expectedToken);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(new { Token = expectedToken });
        }
    }
}