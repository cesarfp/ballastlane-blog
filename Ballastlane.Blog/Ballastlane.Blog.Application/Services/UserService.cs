using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
using Ballastlane.Blog.Domain.Entities;

namespace Ballastlane.Blog.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegistrationResult> RegisterUserAsync(RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            
            if (existingUser != null)
            {
                return new RegistrationResult { IsSuccess = false, Message = "User already exists." };
            }
           
            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                Password = hashedPassword
            };

            await _userRepository.AddAsync(newUser);

            return new RegistrationResult { IsSuccess = true, UserId = newUser.Id };
        }
    }
}
