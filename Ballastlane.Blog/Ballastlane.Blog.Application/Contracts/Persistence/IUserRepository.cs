using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
