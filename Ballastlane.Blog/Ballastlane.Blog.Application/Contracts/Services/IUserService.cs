using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public  interface IUserService
    {
        public Task<RegistrationResult> RegisterUserAsync(RegisterUserRequest request);
        Task<User?> ValidateUserCredentialsAsync(string email, string password);
    }
}
