using Ballastlane.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public interface IPostService
    {
        Task<Post?> GetPostAsync(int id);
        Task<IList<Post>> GetPostsAsync();
        Task<Post> CreatePostAsync(Post post);
        Task<bool> DeletePostAsync(int id);
    }
}
