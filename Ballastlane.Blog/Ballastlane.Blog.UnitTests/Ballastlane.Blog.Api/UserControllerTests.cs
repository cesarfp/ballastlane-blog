using AutoFixture;
using AutoFixture.AutoMoq;
using Ballastlane.Blog.Api.Controllers;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Api
{
    public class UserControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockUserService = _fixture.Freeze<Mock<IUserService>>();
            _controller = new UserController(_mockUserService.Object);
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
    }
}