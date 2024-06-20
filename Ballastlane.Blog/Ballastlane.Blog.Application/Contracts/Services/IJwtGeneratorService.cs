using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Services
{
    public interface IJwtGeneratorService
    {
        string GenerateToken(User user);
    }
}
