using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService, 
            IJwtGeneratorService tokenService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.RegisterUserAsync(request);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password);

                if (!result.IsSuccess)
                {
                    return Unauthorized(result.Message);
                }

                return Ok(new { Token = result.Value });
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            
        }
    }
}
