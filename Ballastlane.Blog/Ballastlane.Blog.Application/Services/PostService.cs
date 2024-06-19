using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository) 
        { 
            _postRepository = postRepository;
        }

        public async Task<Post?> GetPostAsync(int Id)
        {
            return await _postRepository.GetPostAsync(Id);
        }

        public async Task<IList<Post>> GetPostsAsync()
        {
            return await _postRepository.GetPostsAsync();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            return await _postRepository.CreatePostAsync(post);
        }

        public void UpdatePostAsync()
        {
            throw new NotImplementedException();
        }

        public void DeletePostAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
