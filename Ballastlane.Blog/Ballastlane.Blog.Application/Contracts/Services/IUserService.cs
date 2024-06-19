using Ballastlane.Blog.Api.Dtos;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public  interface IUserService
    {
        public Task<RegistrationResult> RegisterUserAsync(RegisterUserRequest request);
    }
}
