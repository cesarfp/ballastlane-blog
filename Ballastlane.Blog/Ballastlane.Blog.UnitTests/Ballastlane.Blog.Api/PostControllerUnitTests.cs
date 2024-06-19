using AutoFixture;
using Ballastlane.Blog.Api.Controllers;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Api
{
    public class PostControllerUnitTests
    {
        private readonly Mock<IPostService> _postServiceMock = new Mock<IPostService>();
        private readonly PostController _controller;
        private readonly Fixture _fixture = new Fixture();

        public PostControllerUnitTests()
        {
            _controller = new PostController(_postServiceMock.Object);
        }

        [Fact]
        public async Task GetPostAsync_PostExists_ReturnsOkObjectResultWithPost()
        {
            // Arrange
            var expectedPost = _fixture.Create<Post>();

            _postServiceMock.Setup(service => service.GetPostAsync(expectedPost.Id)).ReturnsAsync(expectedPost);

            // Act
            var result = await _controller.GetPostAsync(expectedPost.Id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var postResponse = okResult.Value.Should().BeAssignableTo<GetPostResponse>().Subject;
            
            postResponse.Should().BeEquivalentTo(expectedPost, options => options.ExcludingMissingMembers()); 
        }

        [Fact]
        public async Task GetPostAsync_PostDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var postId = _fixture.Create<int>();
            _postServiceMock.Setup(service => service.GetPostAsync(postId)).ReturnsAsync((Post?)null);

            // Act
            var result = await _controller.GetPostAsync(postId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsOkWithPosts()
        {
            // Arrange
            var posts = _fixture.CreateMany<Post>(); 
            _postServiceMock.Setup(service => service.GetPostsAsync()).ReturnsAsync(posts.ToList());

            // Act
            var result = await _controller.GetPostsAsync();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<IEnumerable<GetPostsResponse>>();
            
            var returnedPosts = okResult.Value as IEnumerable<GetPostsResponse>;
            returnedPosts.Should().HaveCount(3);
            returnedPosts.Should().BeEquivalentTo(posts, options => options.ComparingByMembers<Post>());
        }

        [Fact]
        public async Task CreatePostAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Title is required");

            var request = new CreatePostRequest();

            // Act
            var result = await _controller.CreatePostAsync(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            
        }

        [Fact]
        public async Task CreatePostAsync_ReturnsOkWithCreatePostResponse_WhenSuccessful()
        {
            // Arrange
            var request = _fixture.Create<CreatePostRequest>();
              
            var post = _fixture.Build<Post>()
                    .With(p => p.Title, request.Title)
                    .With(p => p.Content, request.Content)
                    .Create();


            _postServiceMock.Setup(service => service.CreatePostAsync(It.Is<Post>(_=>_.Title == request.Title && _.Content == request.Content)))
                           .ReturnsAsync(post);

            // Act
            var result = await _controller.CreatePostAsync(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<CreatePostResponse>().Subject;

            Assert.Equal(request.Title, response.Title);
            Assert.Equal(request.Content, response.Content);
        }
    }
}
