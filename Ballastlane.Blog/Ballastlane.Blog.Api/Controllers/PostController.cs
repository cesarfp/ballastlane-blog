using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
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
            if(id == default)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var result = await _postService.GetPostAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }

                return Ok(new GetPostResponse
                {
                    Id = result.Value.Id,
                    Title = result.Value.Title,
                    Content = result.Value.Content,
                    CreatedAt = result.Value.CreatedAt,
                    UpdatedAt = result.Value.UpdatedAt
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostAsync([FromBody] CreatePostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _postService.CreatePostAsync(request);

                if (!result.IsSuccess)
                {
                    return UnprocessableEntity(new { Error = result.Message });
                }

                return Ok(new CreatePostResponse
                {
                    Id = result.Value.Id,
                    Title = result.Value.Title,
                    Content = result.Value.Content,
                    CreatedAt = result.Value.CreatedAt,
                    UpdatedAt = result.Value.UpdatedAt
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsAsync()
        {
            try
            {
                var result = await _postService.GetPostsAsync();

                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Value.Select(_ => new GetPostsResponse
                {
                    Id = _.Id,
                    Title = _.Title,
                    Content = _.Content,
                    CreatedAt = _.CreatedAt,
                    UpdatedAt = _.UpdatedAt
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            if (id == default)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                var result = await _postService.DeletePostAsync(id);

                if (!result.IsSuccess)
                {
                    return UnprocessableEntity(result.Message);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostAsync(int id, [FromBody] UpdatePostRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request == null || id != request.Id)
            {
                return BadRequest("Post ID does not match the request path ID.");
            }

            try
            {
                var result = await _postService.UpdatePostAsync(request);

                if (!result.IsSuccess)
                {
                    return UnprocessableEntity(result.Message);
                }

                return Ok(result.Value);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the post. Please try again later.");
            }
        }


    }
}
