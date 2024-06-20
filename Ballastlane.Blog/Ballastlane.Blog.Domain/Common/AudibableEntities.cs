namespace Ballastlane.Blog.Domain.Common
{
    public abstract class AudibableEntities
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
