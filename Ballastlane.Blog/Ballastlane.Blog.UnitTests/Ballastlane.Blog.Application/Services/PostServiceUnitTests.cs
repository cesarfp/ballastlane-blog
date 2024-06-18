using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ballastlane.Blog.Domain.Entities;
using AutoFixture;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Application.Services
{
    public class PostServiceUnitTests
    {
        public readonly IPostService _postService;
        public readonly Mock<IPostRepository> _postRepositoryMock = new Mock<IPostRepository>();
        public readonly Fixture _fixture = new Fixture();
        public PostServiceUnitTests()
        {
            _postService = new PostService(_postRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPostsAsync_ShouldBeEmpty_WhenThereAreAnyPostCreated()
        {
            // Arrange 
            var posts = 
            _postRepositoryMock.Setup(repo => repo.GetPostsAsync()).ReturnsAsync(new List<Post>());

            // Act
            var result = await _postService.GetPostsAsync();

            // Assert
            result.Should().BeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostsAsync_ShouldNotBeEmpty_WhenPostWereCreated()
        {
            // Arrange 
            var posts = _fixture.CreateMany<Post>().ToList();
            _postRepositoryMock.Setup(_=> _.GetPostsAsync()).ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsAsync();

            // Assert
            result.Should().NotBeEmpty();
        }
    }
}
