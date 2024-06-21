using Ballastlane.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
