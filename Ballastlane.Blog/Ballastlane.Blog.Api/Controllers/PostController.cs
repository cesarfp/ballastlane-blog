using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostAsync(int id)
        {
            var foundPost = await _postService.GetPostAsync(id);

            if (foundPost == null)
            {
                return NotFound();
            }

            return Ok(new GetPostResponse
            {
                Id = foundPost.Id,
                Title = foundPost.Title,
                Content = foundPost.Content,
                CreatedAt = foundPost.CreatedAt,
                UpdatedAt = foundPost.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostAsync([FromBody] CreatePostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _postService.CreatePostAsync(new Post
            {
                Title = request.Title,
                Content = request.Content
            });

            return Ok(new CreatePostResponse
            {
                Id = createdPost.Id,
                Title = createdPost.Title,
                Content = createdPost.Content,
                CreatedAt = createdPost.CreatedAt,
                UpdatedAt = createdPost.UpdatedAt
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsAsync()
        {
            var foundPosts = await _postService.GetPostsAsync();

            return Ok(foundPosts.Select( _=> new GetPostsResponse
            {
                Id = _.Id,
                Title = _.Title,
                Content = _.Content,
                CreatedAt = _.CreatedAt,
                UpdatedAt = _.UpdatedAt
            }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            var result = await _postService.DeletePostAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostAsync(int id, [FromBody] UpdatePostRequest request)
        {
            if (request == null || id != request.Id)
            {
                return BadRequest("Post ID does not match the request path ID.");
            }

            try
            {
                var updatedPost = await _postService.UpdatePostAsync( new Post
                {
                    Id = request.Id,
                    Title = request.Title,
                    Content = request.Content
                });

                if (updatedPost == null)
                {
                    return NotFound($"Post with ID {id} not found.");
                }

                return Ok(updatedPost);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the post. Please try again later.");
            }
        }


    }
}
