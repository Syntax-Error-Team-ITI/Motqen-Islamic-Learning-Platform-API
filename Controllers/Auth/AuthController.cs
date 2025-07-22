using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Services;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UnitOfWork unit, IEmailService emailService, UserManager<User> userManager) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            var registerResult = await unit.AuthRepo.RegisterAsync(userRegisterDto);
            if (!registerResult.Succeeded)
            {
                return BadRequest(registerResult.Message);
            }

            // Send confirmation email that has token containing this user data to validate over in ConfirmEmail endpoint
            await emailService.SendEmailConfirmationAsync(registerResult.AppUser!);

            return Ok(registerResult.Message);
        }

        [HttpPost("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmDTO request)
        {
            var confirmResult = await unit.AuthRepo.ConfirmEmailAsync(request);
            if (!confirmResult.Succeeded)
            {
                return BadRequest(confirmResult.Message);
            }
            return Ok(confirmResult.Message);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var loginResult = await unit.AuthRepo.LoginAsync(model);
            if (!loginResult.Succeeded)
            {
                return BadRequest(loginResult.Message);
            }
            return Ok(loginResult.Message);
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO refreshTokenDto)
        {
            var refreshResult = await unit.AuthRepo.GenerateRefreshTokenAsync(refreshTokenDto);
            if (!refreshResult.Succeeded)
            {
                return BadRequest(refreshResult.Message);
            }
            return Ok(refreshResult);
        }

        // user click on button on login page to get to this end point
        // which will send an email with a link another front end page which is a form to fill in
        // the new password and click on a button to get to the next end point in the process => [HttpPost("reset-password")]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> SendForgotPasswordMail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok("Check you email for password reset link.");
            }

            await emailService.SendPasswordResetEmailAsync(user);

            return Ok("Check you email for password reset link."); //(front-end) link to reset password form/ page
        }


        //after the user submits the form with the new password OnSubmit() request will be sent to this end point
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid request");

            if (model.NewPassword != model.ConfirmNewPassword)
                return BadRequest("Password and confirm password do not match.");

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
                return Ok("Password has been reset successfully.");

            return BadRequest(result.Errors);
        }



    }
}

