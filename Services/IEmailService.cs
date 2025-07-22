using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;

namespace MotqenIslamicLearningPlatform_API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDTO mailDTO);
        Task SendEmailConfirmationAsync(User user);
        Task SendPasswordResetEmailAsync(User user);
    }
}
