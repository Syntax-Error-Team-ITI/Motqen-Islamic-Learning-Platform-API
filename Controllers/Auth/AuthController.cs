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
using MotqenIslamicLearningPlatform_API.Services.Auth;
using MotqenIslamicLearningPlatform_API.Services.Auth.Utilities;
using MotqenIslamicLearningPlatform_API.Services.Email;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;
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
            var registerResult = await authService.RegisterAsync(request);
            if (!registerResult.Succeeded)
            {
                return BadRequest(new { error = registerResult.Message });
            }

            var user = registerResult.AppUser;

            var roleResult = await userManager.AddToRoleAsync(user, UserRoles.Student);

            user.Student = new Student
            {
                Pic = "tempUrl",
                Gender = request.Gender,
                BirthDate = (DateTime)request.BirthDate,
                Nationality = request.Nationality,
                //Age = DateTime.Today.Year - request.BirthDate.Value.Year,
                ParentNationalId = request.ParentNationalId
            };
            //if (request.BirthDate.Value.Date > DateTime.Today.AddYears(-user.Student.Age))
            //{
            //    user.Student.Age--;
            //}

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { error = updateResult.Errors });

            await authService.SetParentRelation(user.Student);

            await emailService.SendEmailConfirmationAsync(registerResult.AppUser!);

            return Ok(new { message = registerResult.Message });
        }

        [HttpPost("register-parent")]
        public async Task<IActionResult> RegisterAsParent([FromBody] ParentRegisterDTO request)
        {
            var registerResult = await authService.RegisterAsync(request);
            if (!registerResult.Succeeded)
            {
                return BadRequest(new { error = registerResult.Message });
            }

            var user = registerResult.AppUser;

            var roleResult = await userManager.AddToRoleAsync(user, UserRoles.Parent);


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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var loginResult = await authService.LoginAsync(model);
            if (!loginResult.Succeeded)
            {
                return BadRequest(new { error = loginResult.Message });
            }
            return Ok(new { message = loginResult.Message });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO refreshTokenDto)
        {
            var refreshResult = await authService.GenerateRefreshTokenAsync(refreshTokenDto);
            if (!refreshResult.Succeeded)
            {
                return BadRequest(refreshResult.Message);
            }
            return Ok(refreshResult);
        }


        //[HttpPost("add-parent")]
        //public async Task<IActionResult> AddParent(string studentEmail, string parentEmail, string parentNationalId)
        //{
        //    // 1 check if the student exists
        //    var userStudent = await db.Users
        //        .Include(u => u.Student)
        //        .SingleOrDefaultAsync(u => u.Email == studentEmail);

        //    if (userStudent == null || userStudent.Student == null || !(await userManager.IsEmailConfirmedAsync(userStudent)))
        //        return BadRequest("Student not found or not registered.");

        //    // 2 check if the parent exists
        //    var userParent = await db.Users
        //        .Include(u => u.Parent)
        //        .SingleOrDefaultAsync(u => u.Email == parentEmail);

        //    if (userParent == null || userParent.Parent == null || !(await userManager.IsEmailConfirmedAsync(userParent)))
        //        return BadRequest("Parent not found or not registered.");

        //    // 3 check if the parent is already linked to the student
        //    if (userStudent.Student.ParentNationalId == userParent.Parent.NationalId)
        //        return BadRequest("This parent is already linked to the student.");

        //    // 4 link the parent to the student
        //    userStudent.Student.ParentNationalId = userParent.Parent.NationalId;
        //    userStudent.Student.Parent = userParent.Parent;

        //    // 5 update the student in the database
        //    var updateResult = await userManager.UpdateAsync(userStudent);
        //    if (!updateResult.Succeeded)
        //        return BadRequest(updateResult.Errors);

        //    return Ok("Parent " + userParent.FirstName + " added to the student " + userStudent.FirstName + " successfully.");
        //}

        //[HttpPost("resend-confirmation-email")]
        //public async Task<IActionResult> ResendConfirmationEmail(string userId)
        //{
        //    //var user = await userManager.FindByEmailAsync(userId);
        //    var user = await userManager.FindByIdAsync(userId);

        //    if (user == null || (await userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        return BadRequest(new { gg = "User not found or already confirmed." });
        //    }
        //    await emailService.SendEmailConfirmationAsync(user);
        //    return Ok(new { gg = "Confirmation email sent successfully." });
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

        //// user click on button on login page to get to this end point
        //// which will send an email with a link another front end page which is a form to fill in
        //// the new password and click on a button to get to the next end point in the process => [HttpPost("reset-password")]
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> SendForgotPasswordMail(string email)
        //{
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        // Don't reveal that the user does not exist or is not confirmed
        //        return Ok("Check you email for password reset link.");
        //    }

        //    await emailService.SendPasswordResetEmailAsync(user);

        //    return Ok("Check you email for password reset link."); //(front-end) link to reset password form/ page
        //}

        ////after the user submits the form with the new password OnSubmit() request will be sent to this end point
        //[HttpPost("forgot-password/reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDTO model)
        //{
        //    var user = await userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //        return BadRequest("Invalid request");

        //    if (model.NewPassword != model.ConfirmNewPassword)
        //        return BadRequest("Password and confirm password do not match.");

        //    var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

        //    if (result.Succeeded)
        //        return Ok("Password has been reset successfully.");

        //    return BadRequest(result.Errors);
        //}

        //[HttpPost("continue-registration")]
        //after successful login, the user will be redirected to this endpoint only for the first time
        //public async Task<IActionResult> ContinueRegistration([FromBody] UserContinueRegisterDTO request)
        //{
        //    // 1 check if the request email is valid
        //    var user = await db.Users
        //        .Include(u => u.Student)
        //        .Include(u => u.Parent)
        //        .Include(u => u.Teacher)
        //        .SingleOrDefaultAsync(u => u.Email == request.Email);
        //    if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        //        return BadRequest(new { error = "User not found or not confirmed" });


        //    // 2 check if the user has already completed their registration while not being an Admin or Teacher
        //    if (user.Teacher is not null)
        //        return BadRequest(new { error = "As a " + UserRoles.Teacher + " contact the administration to continue your registration" });
        //    if (user.Student is not null)
        //        return BadRequest(new { error = "You have already completed your registration as a " + UserRoles.Student });
        //    if (user.Parent is not null)
        //        return BadRequest(new { error = "You have already completed your registration as a " + UserRoles.Parent });

        //    // 3 check if the role is valid and not Admin or Teacher
        //    if (request.Role != UserRoles.Student && request.Role != UserRoles.Parent)
        //        return BadRequest(new { error = "You can only register as Student or Parent. For other roles please contact the system administrator." });

        //    // 4 create a new user profile based on the role
        //    switch (request.Role)
        //    {
        //        case UserRoles.Student:
        //            user.Student = new Student
        //            {
        //                Pic = request.Pic,
        //                Gender = request.Gender,
        //                Age = DateTime.Today.Year - request.BirthDate.Value.Year,
        //                BirthDate = (DateTime)request.BirthDate,
        //                Nationality = request.Nationality,
        //                ParentNationalId = request.ParentNationalId
        //            };
        //            if (request.BirthDate.Value.Date > DateTime.Today.AddYears(-user.Student.Age))
        //            {
        //                user.Student.Age--;
        //            }
        //            break;

        //        case UserRoles.Parent:
        //            user.PhoneNumber = request.PhoneNumber;
        //            user.Parent = new Parent
        //            {
        //                Pic = request.Pic,
        //                Address = request.Address,
        //                NationalId = request.NationalId
        //            };
        //            break;
        //    }
        //    // 5 update the user in the database
        //    var updateResult = await userManager.UpdateAsync(user);
        //    if (!updateResult.Succeeded)
        //        return BadRequest(new { error = updateResult.Errors });

        //    return Ok(new { pk = updateResult });
        //}

    }
}

