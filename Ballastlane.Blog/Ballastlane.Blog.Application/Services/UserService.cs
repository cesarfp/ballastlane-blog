using Ballastlane.Blog.Api.Dtos;
using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Application.Contracts.Services;
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

        public async Task<RegistrationResult> RegisterUserAsync(RegisterUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            
            if (existingUser != null)
            {
                return new RegistrationResult { IsSuccess = false, Message = "User already exists." };
            }
           
            

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password
            };

            await _userRepository.AddAsync(newUser);

            return new RegistrationResult { IsSuccess = true, UserId = newUser.Id };
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
