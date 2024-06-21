using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Models;
using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public interface IPostService
    {
        Task<Result<Post>> GetPostAsync(int id);
        Task<Result<IList<Post>>> GetPostsAsync();
        Task<Result<Post>> CreatePostAsync(CreatePostRequest post);
        Task<bool> DeletePostAsync(int id);
        Task<Post?> UpdatePostAsync(UpdatePostRequest post);
    }
}
