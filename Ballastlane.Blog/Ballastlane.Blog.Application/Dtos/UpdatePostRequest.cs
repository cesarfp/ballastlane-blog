using System.ComponentModel.DataAnnotations;

namespace Ballastlane.Blog.Api.Dtos
{
    public class UpdatePostRequest
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [MinLength(5)]
        public string Content { get; set; } = string.Empty;
    }
}
