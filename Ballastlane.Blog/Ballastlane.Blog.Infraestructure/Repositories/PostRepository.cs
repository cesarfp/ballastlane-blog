using Ballastlane.Blog.Application.Contracts.Persistence;
using Ballastlane.Blog.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Infraestructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly string _connectionString;

        public PostRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IList<Post>> GetPostsAsync()
        {
            var posts = new List<Post>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Id, Title, Content FROM Post", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var post = new Post
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content"))
                            };

                            posts.Add(post);
                        }
                    }
                }
            }

            return posts;
        }

        
        public async Task<Post> CreatePostAsync(Post post)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO Post (Title, Content) VALUES (@Title, @Content); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@Title", post.Title);
                    command.Parameters.AddWithValue("@Content", post.Content);

                    post.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }

            return post;
        }

    }
}
