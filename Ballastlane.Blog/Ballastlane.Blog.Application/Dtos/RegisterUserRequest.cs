using System.ComponentModel.DataAnnotations;

namespace Ballastlane.Blog.Api.Dtos
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        
        public string Password { get; set; } = string.Empty;
    }
}
