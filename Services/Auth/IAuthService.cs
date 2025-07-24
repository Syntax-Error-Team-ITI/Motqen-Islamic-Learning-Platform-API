using MotqenIslamicLearningPlatform_API.Authorization;
using MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(UserRegisterDTO userRegisterDto);
        Task<AuthResult> ConfirmEmailAsync(EmailConfirmDTO model);
        Task<AuthResult> LoginAsync(UserLoginDTO userLoginDto);
        Task<TokenDTO> GenerateTokenAsync(User user);
        Task<AuthResult> GenerateRefreshTokenAsync(TokenDTO request);
        //Task<AuthResult> ChangePassword(UserChangePasswordDTO userChangePasswordDto);
        //Task<AuthResult> ResetPassword(UserResetPasswordDTO userResetPasswordDto);
        //Task<AuthResult> ForgotPassword(UserForgotPasswordDTO userForgotPasswordDto);
        //Task<AuthResult> VerifyEmail(UserVerifyEmailDTO userVerifyEmailDto);
        //Task<AuthResult> DeleteAccount(UserDeleteAccountDTO userDeleteAccountDto);
    }
}
