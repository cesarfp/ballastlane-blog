using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserContextService _userContextService;

        public PostService(
            IPostRepository postRepository, 
            IUserContextService userContextService) 
        { 
            _postRepository = postRepository;
            _userContextService = userContextService;
        }

        public async Task<Post?> GetPostAsync(int Id)
        {
            var userId = _userContextService.GetCurrentUserId();
            return await _postRepository.GetPostAsync(Id, userId);
        }

        public async Task<IList<Post>> GetPostsAsync()
        {
            var userId = _userContextService.GetCurrentUserId();


            return await _postRepository.GetPostsAsync(userId);
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            var userId = _userContextService.GetCurrentUserId();

            return await _postRepository.CreatePostAsync(post, userId);
        }

        public void UpdatePostAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var userId = _userContextService.GetCurrentUserId();
            var postToDelete = await _postRepository.GetPostAsync(id, userId);

            if (postToDelete == null)
            {
                return false;
            }

            return await _postRepository.DeletePostAsync(id, userId);
        }

        public async Task<Post?> UpdatePostAsync(Post post)
        {
            ArgumentNullException.ThrowIfNull(post, nameof(post));

            var userId = _userContextService.GetCurrentUserId();
            var existingPost = await _postRepository.GetPostAsync(post.Id, userId);
            if (existingPost == null)
            {
                return null;
            }

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
           
            await _postRepository.UpdatePostAsync(existingPost, userId);

            return existingPost;
        }
    }
}
