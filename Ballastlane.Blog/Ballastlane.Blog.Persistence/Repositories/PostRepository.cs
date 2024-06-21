using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Text;

namespace Ballastlane.Blog.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly string _connectionString;

        public PostRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Post?> GetPostAsync(int id, int userId)
        {
            Post? post = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id, Title, Content, CreatedAt, UpdatedAt FROM Post WHERE Id = @Id AND UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            post = new Post
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            };
                        }
                    }
                }
            }

            return post;
        }

        public async Task<IList<Post>> GetPostsAsync(int? userId = null)
        {
            var posts = new List<Post>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sqlQuery = new StringBuilder("SELECT Id, Title, Content, CreatedAt, UpdatedAt FROM Post");
                if (userId.HasValue)
                {
                    sqlQuery.Append(" WHERE UserId = @UserId");
                }

                using (var command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    if (userId.HasValue)
                    {
                        command.Parameters.AddWithValue("@UserId", userId.Value);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var post = new Post
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                            };

                            posts.Add(post);
                        }
                    }
                }
            }

            return posts;
        }


        public async Task<Post> CreatePostAsync(Post post, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string insertSql = @"
                    INSERT INTO Post (Title, Content, UserId) 
                    VALUES (@Title, @Content, @UserId); 
                    SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Title", post.Title);
                    command.Parameters.AddWithValue("@Content", post.Content);
                    command.Parameters.AddWithValue("@UserId", userId);

                    post.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }

                if (post.Id > 0)
                {
                    string selectSql = "SELECT CreatedAt FROM Post WHERE Id = @Id";

                    using (var command = new SqlCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", post.Id);

                        var createdAtObj = await command.ExecuteScalarAsync();
                        if (createdAtObj != null)
                        {
                            post.CreatedAt = Convert.ToDateTime(createdAtObj);
                        }
                    }
                }
            }

            return post;
        }

        public async Task<bool> DeletePostAsync(int id, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM Post WHERE Id = @Id AND UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@UserId", userId);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> UpdatePostAsync(Post post, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = new StringBuilder("UPDATE Post SET ");
                query.Append("Title = @Title, ");
                query.Append("Content = @Content, ");
                query.Append("UpdatedAt = @UpdatedAt ");
                query.Append("WHERE Id = @Id AND UserId = @UserId");

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@Title", post.Title);
                    command.Parameters.AddWithValue("@Content", post.Content);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@Id", post.Id);
                    command.Parameters.AddWithValue("@UserId", userId);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }



    }
}
