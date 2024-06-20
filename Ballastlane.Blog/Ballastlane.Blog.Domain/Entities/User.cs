using Ballastlane.Blog.Domain.Common;

namespace Ballastlane.Blog.Domain.Entities
{
    public class User : AudibableEntities
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
