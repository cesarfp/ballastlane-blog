﻿using Ballastlane.Blog.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ballastlane.Blog.Infraestructure.Services
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
