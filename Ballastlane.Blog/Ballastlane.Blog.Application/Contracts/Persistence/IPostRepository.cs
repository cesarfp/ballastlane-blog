using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Persistence
{
    public interface IPostRepository
    {
        Task<Post?> GetPostAsync(int Id, int userId);
        Task<IList<Post>> GetPostsAsync(int? userId = null);
        Task<Post> CreatePostAsync(Post post, int userId);
        Task<bool> DeletePostAsync(int id, int userId);
        Task<Post?> UpdatePostAsync(Post post, int userId);

    }
}
