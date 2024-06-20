using AutoFixture;
using Ballastlane.Blog.Api.Controllers;
using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ballastlane.Blog.UnitTests.Ballastlane.Blog.Api
{
    public class PostControllerTests
    {
        private readonly Mock<IPostService> _postServiceMock = new Mock<IPostService>();
        private readonly PostController _controller;
        private readonly Fixture _fixture = new Fixture();

        public PostControllerTests()
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

        [Fact]
        public async Task DeletePostAsync_PostExists_ReturnsNoContent()
        {
            // Arrange
            var postId = 1;
            _postServiceMock.Setup(service => service.DeletePostAsync(postId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePostAsync(postId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeletePostAsync_PostDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var postId = 1;
            _postServiceMock.Setup(service => service.DeletePostAsync(postId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePostAsync(postId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var request = _fixture.Build<UpdatePostRequest>()
                                  .With(x => x.Id, 2)
                                  .Create();

            // Act
            var result = await _controller.UpdatePostAsync(1, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            _postServiceMock.Setup(service => service.UpdatePostAsync(It.IsAny<Post>()))
                            .ReturnsAsync((Post?)null);

            var request = _fixture.Create<UpdatePostRequest>();

            // Act
            var result = await _controller.UpdatePostAsync(request.Id, request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var post = _fixture.Create<Post>();
            _postServiceMock.Setup(service => service.UpdatePostAsync(It.IsAny<Post>()))
                            .ReturnsAsync(post);

            var request = _fixture.Build<UpdatePostRequest>()
                                  .With(x => x.Id, post.Id)
                                  .With(x => x.Title, post.Title)
                                  .With(x => x.Content, post.Content)
                                  .Create();

            // Act
            var result = await _controller.UpdatePostAsync(post.Id, request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(post, options => options.ComparingByMembers<Post>());
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsStatusCode500_WhenExceptionOccurs()
        {
            // Arrange
            _postServiceMock.Setup(service => service.UpdatePostAsync(It.IsAny<Post>()))
                            .ThrowsAsync(new Exception("Test exception"));

            var request = _fixture.Create<UpdatePostRequest>();

            // Act
            var result = await _controller.UpdatePostAsync(request.Id, request);

            // Assert
            var statusCodeResult = result as ObjectResult;
            statusCodeResult?.StatusCode.Should().Be(500);
        }
    }
}
