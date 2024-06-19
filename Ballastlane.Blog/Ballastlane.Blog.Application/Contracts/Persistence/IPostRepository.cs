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
        public Task<IList<Post>> GetPostsAsync();
        public Task<Post> CreatePostAsync(Post post);
    }
}
