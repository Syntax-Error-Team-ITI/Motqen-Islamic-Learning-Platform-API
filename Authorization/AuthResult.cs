using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Authorization
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public User? AppUser { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public AuthResult()
        {
            Succeeded = false;
            Message = string.Empty;
        }
    }
}
