using Ballastlane.Blog.Api.Dtos;
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
        private readonly IJwtGeneratorService _tokenService;

        public UserController(
            IUserService userService, 
            IJwtGeneratorService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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
                    return Ok(result);
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
            var user = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
