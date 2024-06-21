using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Models;
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

        public async Task<Result<Post>> GetPostAsync(int Id)
        {
            var userId = _userContextService.GetCurrentUserId();

            if(userId == default)
            {
                return Result<Post>.Failure("User not found.");
            }

            var post = await _postRepository.GetPostAsync(Id, userId);

            if(post == null)
            {
                return Result<Post>.Failure("Post not found.");
            }

            return Result<Post>.Success(post);
        }

        public async Task<Result<IList<Post>>> GetPostsAsync()
        {
            var userId = _userContextService.GetCurrentUserId();
            
            if (userId == default)
            {
                return Result<IList<Post>>.Failure("User not found.");
            }

            var posts = await _postRepository.GetPostsAsync(userId);


            return Result<IList<Post>>.Success(posts);
        }

        public async Task<Result<Post>> CreatePostAsync(CreatePostRequest request)
        {
            if (request == null)
            {
                return Result<Post>.Failure("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
            {
                return Result<Post>.Failure("Title and content cannot be empty.");
            }

            var userId = _userContextService.GetCurrentUserId();

            if (userId == default)
            {
                return Result<Post>.Failure("User not found.");
            }


            var post = await _postRepository.CreatePostAsync(new Post
            {
                Title = request.Title,
                Content = request.Content
            }, userId);

            return Result<Post>.Success(post);
        }

        public async Task<Result<bool>> DeletePostAsync(int id)
        {
            if(id == default)
            {
                return Result<bool>.Failure("Invalid ID.");
            }

            var userId = _userContextService.GetCurrentUserId();
            
            if (userId == default)
            {
                return Result<bool>.Failure("User not found.");
            }

            var postToDelete = await _postRepository.GetPostAsync(id, userId);

            if (postToDelete == null)
            {
                return Result<bool>.Failure("Post not found.");
            }

            return Result<bool>.Success(await _postRepository.DeletePostAsync(id, userId));
        }

        public async Task<Result<Post?>> UpdatePostAsync(UpdatePostRequest request)
        {
            var userId = _userContextService.GetCurrentUserId();

            if (userId == default)
            {
                return Result<Post?>.Failure("User not found.");
            }

            var existingPost = await _postRepository.GetPostAsync(request.Id, userId);
            if (existingPost == null)
            {
                return Result<Post?>.Failure("Post not found.");
            }

            existingPost = await _postRepository.UpdatePostAsync(existingPost, userId);

            if (existingPost == null)
            {
                return Result<Post?>.Failure("An error occurred while updating the post.");
            }

            return Result<Post?>.Success(existingPost);
        }
    }
}
