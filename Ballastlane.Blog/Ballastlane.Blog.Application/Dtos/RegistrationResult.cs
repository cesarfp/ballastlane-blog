namespace Ballastlane.Blog.Api.Dtos
{
    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
