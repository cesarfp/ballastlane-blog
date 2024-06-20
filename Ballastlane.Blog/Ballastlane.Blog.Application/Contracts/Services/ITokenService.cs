using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
