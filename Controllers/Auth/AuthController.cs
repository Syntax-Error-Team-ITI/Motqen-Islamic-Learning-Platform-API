using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.Authorization;
using MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using MotqenIslamicLearningPlatform_API.Services.Email;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UnitOfWork unit, IEmailService emailService, UserManager<User> userManager) : ControllerBase
    {
        [HttpPost("register")]
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

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmDTO request)
        {
            var confirmResult = await unit.AuthRepo.ConfirmEmailAsync(request);
            if (!confirmResult.Succeeded)
            {
                return BadRequest(confirmResult.Message);
            }
            return Ok(confirmResult.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var loginResult = await unit.AuthRepo.LoginAsync(model);
            if (!loginResult.Succeeded)
            {
                return BadRequest(loginResult.Message);
            }
            return Ok(loginResult.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO refreshTokenDto)
        {
            var refreshResult = await unit.AuthRepo.GenerateRefreshTokenAsync(refreshTokenDto);
            if (!refreshResult.Succeeded)
            {
                return BadRequest(refreshResult.Message);
            }
            return Ok(refreshResult);
        }

        [HttpPost("continue-registration")]
        public async Task<IActionResult> ContinueRegistration([FromBody] UserContinueRegisterDTO request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Invalid request");

            // Update user properties based on the request
            switch (request.Role)
            {
                case UserRoles.Teacher:
                    user.Teacher = new Teacher
                    {
                        Pic = request.Pic,
                        Gender = request.Gender,
                        Age = (int)request.Age
                    };
                    break;

                case UserRoles.Student:
                    user.Student = new Student
                    {
                        Pic = request.Pic,
                        Gender = request.Gender,
                        Age = (int)request.Age,
                        BirthDate = (DateTime)request.BirthDate,
                        Nationality = request.Nationality
                    };
                    break;

                case UserRoles.Parent:
                    user.PhoneNumber = request.PhoneNumber;
                    user.Parent = new Parent
                    {
                        Pic = request.Pic,
                        Address = request.Address
                    };
                    break;
            }
            // Update the user in the database
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] string email, string oldPassword, string newPassword, string confirmNewPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid request");
            if (newPassword != confirmNewPassword)
                return BadRequest("New password and confirm new password do not match.");
            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
                return Ok("Password has been changed successfully.");
            return BadRequest(result.Errors);
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

