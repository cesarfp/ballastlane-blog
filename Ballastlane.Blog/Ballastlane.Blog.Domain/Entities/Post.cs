using Ballastlane.Blog.Domain.Common;

namespace Ballastlane.Blog.Domain.Entities
{
    public class Post : AudibableEntities
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
