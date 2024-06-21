using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Infraestructure;
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

        public async Task<Post> CreatePostAsync(CreatePostRequest request)
        {
            var userId = _userContextService.GetCurrentUserId();

            var post = new Post
            {
                Title = request.Title,
                Content = request.Content
            };

            return await _postRepository.CreatePostAsync(post, userId);
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

        public async Task<Post?> UpdatePostAsync(UpdatePostRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var userId = _userContextService.GetCurrentUserId();
            var existingPost = await _postRepository.GetPostAsync(request.Id, userId);
            if (existingPost == null)
            {
                return null;
            }

            existingPost.Title = request.Title;
            existingPost.Content = request.Content;
           
            await _postRepository.UpdatePostAsync(existingPost, userId);

            return existingPost;
        }
    }
}
