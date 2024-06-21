using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Ballastlane.Blog.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT Id, Email, Password, CreatedAt, UpdatedAt FROM [User] WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            };
                        }
                    }
                }
            }

            return user;
        }

        public async Task AddAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("INSERT INTO [User] (Email, Password) VALUES (@Email, @Password)", connection))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
