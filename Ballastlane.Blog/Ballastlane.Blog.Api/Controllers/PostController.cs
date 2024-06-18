using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        //endpoint to create a new post
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostRequest post)
        {
            //create post
            //return Ok();
            throw new NotImplementedException();
        }

        //Get a all posts
        [HttpGet]
        public async Task<IActionResult> GetPostsAsync()
        {
            //get all posts
            var posts = await _postService.GetPostsAsync();
            return Ok(posts);
        }
    }
}
