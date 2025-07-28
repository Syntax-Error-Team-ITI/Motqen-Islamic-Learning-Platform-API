using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MotqenIslamicLearningPlatform_API.Authorization;
using MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs;
using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
using MotqenIslamicLearningPlatform_API.Models;
using MotqenIslamicLearningPlatform_API.Models.ParentModel;
using MotqenIslamicLearningPlatform_API.Models.Shared;
using MotqenIslamicLearningPlatform_API.Models.StudentModel;
using MotqenIslamicLearningPlatform_API.Models.TeacherModel;
using MotqenIslamicLearningPlatform_API.Services.Auth;
using MotqenIslamicLearningPlatform_API.Services.Auth.Utilities;
using MotqenIslamicLearningPlatform_API.Services.Email;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MotqenIslamicLearningPlatform_API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        //UnitOfWork unit,
        IEmailService emailService,
        IAuthService authService,
        UserManager<User> userManager,
        MotqenDbContext db
        ) : ControllerBase
    {
        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterAsStudent([FromBody] StudentRegisterDTO request)
        {
            User user;
            AuthResult registerResult = new();

            if (false)
            //if (request.GoogleIdToken != null)
            {
                //var payload = await authService.VerifyGoogleToken(request.GoogleIdToken);

                //user = new User
                //{
                //    Email = payload.Email,
                //    UserName = payload.Email,
                //    FirstName = request.FirstName ?? payload.GivenName,
                //    LastName = request.LastName ?? payload.FamilyName,
                //    EmailConfirmed = true
                //};

                //var result = await userManager.CreateAsync(user);
                //if (!result.Succeeded)
                //    return BadRequest(new { error = string.Join(", ", result.Errors.Select(e => e.Description)) });

                //registerResult.Message = result.Succeeded.ToString();
            }
            else
            {
                registerResult = await authService.RegisterAsync(request);
                if (!registerResult.Succeeded)
                    return BadRequest(new { error = registerResult.Message });
                user = registerResult.AppUser;
            }

            await userManager.AddToRoleAsync(user, UserRoles.Student);
            user.Student = new Student
            {
                Pic = "tempUrl",
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                Nationality = request.Nationality,
                //Age = make function to set age
                ParentNationalId = request.ParentNationalId
            };

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { error = updateResult.Errors });

            await authService.SetParentRelation(user.Student);

            await emailService.SendEmailConfirmationAsync(user);

            return Ok(new { message = registerResult.Message });
        }

        [HttpPost("register-parent")]
        public async Task<IActionResult> RegisterAsParent([FromBody] ParentRegisterDTO request)
        {
            if (await authService.CheckNationalIdUniqueness(request.NationalId))
                return BadRequest(new { error = "A parent with this national Id is already registered" });

            var registerResult = await authService.RegisterAsync(request);
            if (!registerResult.Succeeded)
            {
                return BadRequest(new { error = registerResult.Message });
            }

            var user = registerResult.AppUser;

            await userManager.AddToRoleAsync(user, UserRoles.Parent);

            user.Parent = new Parent
            {
                Pic = "tempUrl",
                NationalId = request.NationalId,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { error = updateResult.Errors });

            await authService.SetChildrenRelation(user.Parent);

            await emailService.SendEmailConfirmationAsync(registerResult.AppUser!);

            return Ok(new { message = registerResult.Message });
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost("add-teacher")]
        public async Task<IActionResult> AddTeacher(AddTeacherDTO request)
        {
            var registerResult = await authService.RegisterAsync(request);
            if (!registerResult.Succeeded)
            {
                return BadRequest(new { error = registerResult.Message });
            }

            var user = registerResult.AppUser;

            user.EmailConfirmed = true;

            await userManager.AddToRoleAsync(user, UserRoles.Teacher);

            user.Teacher = new Teacher
            {
                Pic = "tempUrl",
                Gender = request.Gender,
                Age = request.Age
            };

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { error = updateResult.Errors });

            return Ok(new { message = "Teacher added successfully" });
        }

        // this endpoint is only accessed through the link in the email sent to the user after registration
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            //var decodedToken = WebUtility.UrlDecode(token);
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var request = new EmailConfirmDTO() { UserId = userId, Token = decodedToken };

            var confirmResult = await authService.ConfirmEmailAsync(request);
            if (!confirmResult.Succeeded)
            {
                return BadRequest(new { error = confirmResult.Message });
            }
            return Ok(new { message = confirmResult.Message });
        }


        [HttpGet("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            //var user = await userManager.FindByIdAsync(userId);

            if (user == null || (await userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest(new { gg = "User not found or already confirmed." });
            }
            await emailService.SendEmailConfirmationAsync(user);
            return Ok(new { gg = "Confirmation email sent successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var loginResult = await authService.LoginAsync(model);
            if (!loginResult.Succeeded)
            {
                return BadRequest(new { error = loginResult.Message });
            }

            Response.Cookies.Append("refreshToken", loginResult.RefreshToken!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // HTTPS only
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh-token", // Only sent to refresh endpoint
                Expires = DateTime.UtcNow.AddDays(7)
            });

            //return Ok(new { accessToken = loginResult.AccessToken , refreshToken = loginResult.RefreshToken });
            return Ok(new { accessToken = loginResult.AccessToken });
        }


        // user click on button on login page to get to this end point
        // which will send an email with a link another front end page which is a form to fill in
        // the new password and click on a button to get to the next end point in the process => [HttpPost("reset-password")]
        [HttpGet("forgot-password")]
        public async Task<IActionResult> SendForgotPasswordMail([FromQuery] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return Ok(new { mesage = "Check you email for password reset link." });
            }

            await emailService.SendPasswordResetEmailAsync(user);

            return Ok(new { mesage = "Check you email for password reset link." }); //(front-end) link to reset password form/ page
        }

        //after the user submits the form with the new password OnSubmit() request will be sent to this end point
        [HttpPost("forgot-password/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDTO model)
        {
            var user = await userManager.FindByIdAsync(model.userId);
            if (user == null)
                return BadRequest(new { error = "Invalid request" });

            if (model.NewPassword != model.ConfirmNewPassword)
                return BadRequest(new { error = "Password and confirm password do not match." });

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Succeeded)
                return Ok(new { message = "Password has been reset successfully." });

            return BadRequest(new { error = result.Errors });
        }



        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken([FromBody] TokenDTO refreshTokenDto)
        //{
        //    var refreshResult = await authService.GenerateRefreshTokenAsync(refreshTokenDto);
        //    if (!refreshResult.Succeeded)
        //    {
        //        return BadRequest(refreshResult.Message);
        //    }
        //    return Ok(refreshResult);
        //}


        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] string email, string oldPassword, string newPassword, string confirmNewPassword)
        //{
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        //        return BadRequest("Invalid request, or user email not confirmed");
        //    if (newPassword != confirmNewPassword)
        //        return BadRequest("New password and confirm new password do not match.");
        //    var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        //    if (result.Succeeded)
        //        return Ok("Password has been changed successfully.");
        //    return BadRequest(result.Errors);
        //}




    }
}

