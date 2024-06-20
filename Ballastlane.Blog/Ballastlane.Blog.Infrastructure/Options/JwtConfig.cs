namespace Ballastlane.Blog.Infrastructure.Options
{
    public class JwtConfig
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 3;
    }
}
