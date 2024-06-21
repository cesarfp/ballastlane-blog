using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Contracts.Infraestructure
{
    public interface IJwtGeneratorService
    {
        string GenerateToken(User user);
    }
}
