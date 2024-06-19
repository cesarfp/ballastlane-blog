﻿namespace Ballastlane.Blog.Api.Dtos
{
    public class CreatePostResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
