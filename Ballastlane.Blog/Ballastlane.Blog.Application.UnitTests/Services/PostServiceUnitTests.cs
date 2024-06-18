using Ballastlane.Blog.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Application.UnitTests.Services
{
    internal class PostServiceUnitTests
    {
        private PostService _postService = new PostService();
        
        public PostServiceUnitTests()
        {
        }

        public void GetPosts_ShouldReturnAllPosts()
        {
            // Arrange & act
            var posts = _postService.GetPosts();

            // Assert
            //posts.Should
        }
    }
}
