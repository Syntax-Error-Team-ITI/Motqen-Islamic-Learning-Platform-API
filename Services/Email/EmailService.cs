using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace MotqenIslamicLearningPlatform_API.Services.Email
{
    public class EmailService(IConfiguration configuration, UserManager<User> userManager) : IEmailService
    {
        public async Task SendEmailAsync(EmailRequestDTO mailDTO)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(
                configuration["SmtpSettings:SenderName"],
                configuration["SmtpSettings:SenderEmail"]));

            emailMessage.To.Add(new MailboxAddress("", mailDTO.ToEmail));

            emailMessage.Subject = mailDTO.Subject;

            emailMessage.Body = new TextPart("html")
            {
                Text = mailDTO.Body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(
                    configuration["SmtpSettings:Server"],
                    int.Parse(configuration["SmtpSettings:Port"]),
                    MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);

                await client.AuthenticateAsync(
                    configuration["SmtpSettings:Username"],
                    configuration["SmtpSettings:Password"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailConfirmationAsync(User user)
        {
            // generate confirmation token
            var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            //var encodedToken = WebUtility.UrlEncode(emailConfirmToken);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmToken));

            var link = $"{configuration["URLs:ClientUrl"]}/confirm-email?userId={user.Id}&token={encodedToken}";

            // Prepare email
            EmailRequestDTO emailRequest = new EmailRequestDTO
            {
                ToEmail = user.Email,
                Subject = "Confirm your email",
                //Body = $"Please click this link: {link} , to confirm your email]"
                Body = $"Please click <a href='{link}'>this link</a> to confirm your email, {encodedToken}"
            };

            // Send
            await SendEmailAsync(emailRequest);
        }

        public async Task SendPasswordResetEmailAsync(User user)
        {
            // generate reset token
            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            // Prepare email
            EmailRequestDTO emailRequest = new EmailRequestDTO
            {
                ToEmail = user.Email,
                Subject = "Password Reset",
                Body = $"reset token: [{passwordResetToken}]"
            };

            // Send email
            await SendEmailAsync(emailRequest);
        }
    }
}

