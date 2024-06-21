using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Application.Models;
using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> RegisterUserAsync(RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            
            if (existingUser != null)
            {
                return Result.Failure("User already exists.");
            }

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            await _userRepository.AddAsync(newUser);

            return Result.Success("User registered successfully.");
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user != null)
            {
                var isValidPassword = user.Password == password;

                if (isValidPassword)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
