using AutoFixture;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Models;
using Ballastlane.Blog.Application.Services;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Application.Services
{
    public class PostServiceTests
    {
        public readonly IPostService _postService;
        public readonly Mock<IPostRepository> _postRepositoryMock = new Mock<IPostRepository>();
        public readonly Mock<IUserContextService> _userContextServiceMock = new Mock<IUserContextService>();
        public readonly Fixture _fixture = new Fixture();
        public PostServiceTests()
        {
            _postService = new PostService(
                _postRepositoryMock.Object, 
                _userContextServiceMock.Object);
        }

        [Fact]
        public async Task GetPostAsync_WhenPostExists_ReturnsPost()
        {
            // Arrange
            var expectedPost = _fixture.Create<Post>();
            var userId = _fixture.Create<int>();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId); // Mock the user context service to return the expected userId
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(expectedPost.Id, userId)).ReturnsAsync(expectedPost);

            // Act
            var result = await _postService.GetPostAsync(expectedPost.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPost, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetPostAsync_WhenPostDoesNotExist_ReturnsNull()
        {
            // Arrange
            var postId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var resultObject = Result<Post>.Failure("Post not found.");
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(postId, userId)).ReturnsAsync((Post?)null);

            // Act
            var result = await _postService.GetPostAsync(postId);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsAllPosts()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var expectedPosts = _fixture.CreateMany<Post>().ToList();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId);
            _postRepositoryMock.Setup(repo => repo.GetPostsAsync(userId)).ReturnsAsync(expectedPosts);

            // Act
            var result = await _postService.GetPostsAsync();

            // Assert
            result.Value.Should().BeEquivalentTo(expectedPosts);
        }

        [Fact]
        public async Task GetPostsAsync_ShouldBeEmpty_WhenThereAreAnyPostCreated()
        {
            // Arrange 
            var userId = _fixture.Create<int>();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId);
            _postRepositoryMock.Setup(repo => repo.GetPostsAsync(userId)).ReturnsAsync(new List<Post>());

            // Act
            var result = await _postService.GetPostsAsync();

            // Assert
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostsAsync_ShouldNotBeEmpty_WhenPostWereCreated()
        {
            // Arrange 
            var userId = _fixture.Create<int>();
            var posts = _fixture.CreateMany<Post>().ToList();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId);
            _postRepositoryMock.Setup(repo => repo.GetPostsAsync(userId)).ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsAsync();

            // Assert
            result.Value.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeletePostAsync_WhenPostExists_ReturnsTrue()
        {
            // Arrange
            var post = _fixture.Create<Post>();
            var userId = _fixture.Create<int>();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId); // Mock the user context service to return the expected userId
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(post.Id, userId)).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.DeletePostAsync(post.Id, userId)).ReturnsAsync(true);

            // Act
            var result = await _postService.DeletePostAsync(post.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePostAsync_WhenPostDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var postId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(postId, userId)).ReturnsAsync((Post?)null);

            // Act
            var result = await _postService.DeletePostAsync(postId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePostAsync_ThrowsArgumentNullException_WhenPostIsNull()
        {
            // Arrange
            Func<Task> act = async () => await _postService.UpdatePostAsync(null!);

            // Act & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsNull_WhenPostDoesNotExist()
        {
            // Arrange
            var request = _fixture.Create<UpdatePostRequest>();
            var userId = _fixture.Create<int>();
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(It.IsAny<int>(), userId)).ReturnsAsync((Post?)null);

            // Act
            var result = await _postService.UpdatePostAsync(request);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsUpdatedPost_WhenUpdateIsSuccessful()
        {
            // Arrange
            var request = _fixture.Create<UpdatePostRequest>();
            var post = _fixture.Build<Post>()
                              .With(p => p.Id, request.Id)
                              .With(p => p.Title, request.Title)
                              .With(p => p.Content, request.Content)
                              .With(p => p.CreatedAt, _fixture.Create<DateTime>())
                              .With(p => p.UpdatedAt, _fixture.Create<DateTime>())
                              .Create();

            var userId = _fixture.Create<int>();
            _userContextServiceMock.Setup(service => service.GetCurrentUserId()).Returns(userId); // Mock the user context service to return the expected userId
            _postRepositoryMock.Setup(repo => repo.GetPostAsync(request.Id, userId)).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.UpdatePostAsync(It.IsAny<Post>(), userId)).ReturnsAsync(true);

            // Act
            var result = await _postService.UpdatePostAsync(request);

            // Assert
            _postRepositoryMock.Verify(repo => repo.UpdatePostAsync(It.Is<Post>(p => p.Id == request.Id && p.Title == request.Title && p.Content == request.Content), userId), Times.Once);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(request, options => options.ComparingByMembers<Post>());
        }
    }


}
