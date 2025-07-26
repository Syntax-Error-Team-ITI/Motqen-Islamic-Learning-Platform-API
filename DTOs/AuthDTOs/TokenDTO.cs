namespace MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public TokenDTO(string accessToken, string? refreshToken = null, DateTime? accessTokenExpiration = null, DateTime? refreshTokenExpiration = null)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiration = accessTokenExpiration;
            RefreshTokenExpiration = refreshTokenExpiration;
        }
    }
}
