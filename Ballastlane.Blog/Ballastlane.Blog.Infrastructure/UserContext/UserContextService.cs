using Ballastlane.Blog.Application.Contracts.Infraestructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ballastlane.Blog.Infrastructure.UserContext
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            return userId;
        }
    }
}
