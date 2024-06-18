using System.ComponentModel.DataAnnotations;

namespace Ballastlane.Blog.Api.Dtos
{
    public class CreatePostRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
