using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Models;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public  interface IUserService
    {
        public Task<Result> RegisterUserAsync(RegisterUserRequest request);
        Task<Result<string>> ValidateUserCredentialsAsync(string email, string password);
    }
}
