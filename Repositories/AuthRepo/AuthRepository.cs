//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using MotqenIslamicLearningPlatform_API.Authorization;
//using MotqenIslamicLearningPlatform_API.DTOs.AuthDTOs;
//using MotqenIslamicLearningPlatform_API.DTOs.UserDTOs;
//using MotqenIslamicLearningPlatform_API.Models;
//using MotqenIslamicLearningPlatform_API.Models.Shared;
//using System.Data;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace MotqenIslamicLearningPlatform_API.Repositories.AuthRepo
//{
//    public class AuthRepository(
//        MotqenDbContext db,
//        UserManager<User> userManager,
//        RoleManager<IdentityRole> roleManager,
//        IConfiguration configuration
//        ) : IAuthRepository
//    {
//        public async Task<AuthResult> RegisterAsync(UserRegisterDTO userRegisterDto)
//        {
//            var emailExists = await userManager.FindByEmailAsync(userRegisterDto.Email);
//            if (emailExists != null)
//            {
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = "Email is already registered."
//                };
//            }

//            var userExists = await userManager.FindByNameAsync(userRegisterDto.Username);
//            if (userExists != null)
//            {
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = "Username is already taken."
//                };
//            }

//            if (!await roleManager.RoleExistsAsync(userRegisterDto.Role))
//            {
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = $"The role '{userRegisterDto.Role}' does not exist."
//                };
//            }

//            User user = new User()
//            {
//                //SecurityStamp = Guid.NewGuid().ToString(), what is this?
//                Email = userRegisterDto.Email,
//                UserName = userRegisterDto.Username,
//                FirstName = userRegisterDto.FirstName,
//                LastName = userRegisterDto.LastName
//            };

//            //if (userRegisterDto.Role == UserRoles.Student)
//            //{
//            //    user.Student = new Student();
//            //    user.Student.Gender = userRegisterDto.Gender;
//            //}
//            //else if (userRegisterDto.Role == UserRoles.Teacher)
//            //    user.Teacher = new Teacher();
//            //else if (userRegisterDto.Role == UserRoles.Parent)
//            //    user.Parent = new Parent();

//            var result = await userManager.CreateAsync(user, userRegisterDto.Password);
//            if (!result.Succeeded)
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = result.Errors.Select(e => e.Description).ToString()
//                };

//            var roleResult = await userManager.AddToRoleAsync(user, userRegisterDto.Role);
//            if (!roleResult.Succeeded)
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = "Registration failed! Error occurred at role assigning"
//                };

//            return new AuthResult
//            {
//                AppUser = user,
//                Succeeded = true,
//                Message = "User registered successfully, please check your email for verification!"
//            };
//        }

//        public async Task<AuthResult> ConfirmEmailAsync(EmailConfirmDTO model)
//        {
//            var user = await userManager.FindByIdAsync(model.UserId);
//            if (user == null)
//                return new AuthResult { Succeeded = false, Message = "User not found." };

//            var result = await userManager.ConfirmEmailAsync(user, model.Token);
//            if (result.Succeeded)
//                return new AuthResult { Succeeded = true, Message = "Email is confirmed successfully!" };

//            return new AuthResult
//            {
//                Succeeded = false,
//                Message = string.Join(", ", result.Errors.Select(e => e.Description))
//            };
//        }

//        public async Task<AuthResult> LoginAsync(UserLoginDTO model)
//        {
//            var user = await userManager.Users
//                .Include(u => u.Student)
//                .Include(u => u.Teacher)
//                .Include(u => u.Parent)
//                .FirstOrDefaultAsync(u => u.UserName == model.Username);


//            //var roles = await userManager.GetRolesAsync(user);
//            //foreach (var role in roles)
//            //{
//            //    if (role != model.Role)
//            //    {
//            //        return new AuthResult { Succeeded = false, Message = "This user does not have the required role!" };
//            //    }
//            //}

//            if (user == null)
//                return new AuthResult { Succeeded = false, Message = "Invalid username or password!" };
//            if (!await userManager.CheckPasswordAsync(user, model.Password))
//                return new AuthResult { Succeeded = false, Message = "Invalid username or password!" };
//            if (!user.EmailConfirmed)
//                return new AuthResult { Succeeded = false, Message = "Email is not confirmed!" };

//            if (!await userManager.IsInRoleAsync(user, UserRoles.Admin)
//                && !await userManager.IsInRoleAsync(user, UserRoles.Teacher)
//                && user.Student == null
//                && user.Parent == null
//                )
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = "You have not completed your registration yet. Please continue registration."
//                };

//            var tokenDTO = await GenerateTokenAsync(user);

//            return new AuthResult
//            {
//                Succeeded = true,
//                Message = $"Access Token: [{tokenDTO.AccessToken}],"
//                + $"Expiry: [{tokenDTO.AccessTokenExpiration}], "
//                + $" Refresh Token: [{tokenDTO.RefreshToken}]"
//                + $"Expiry: [{tokenDTO.RefreshTokenExpiration}]"
//            };
//        }

//        public async Task<TokenDTO> GenerateTokenAsync(User user)
//        {
//            bool isAdmin = await userManager.IsInRoleAsync(user, UserRoles.Admin);
//            var roles = await userManager.GetRolesAsync(user);

//            var Claims = new List<Claim>
//                {
//                new Claim(ClaimTypes.Name, user.UserName),
//                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), what is this?
//                new Claim(ClaimTypes.Email, user.Email),
//                new Claim("FirstName", user.FirstName),
//                new Claim("LastName", user.LastName),
//                new Claim("IsAdmin" , isAdmin.ToString())
//                };

//            if (user.Student != null)
//                Claims.Add(new Claim("StudentId", user.Student.Id.ToString()));
//            if (user.Teacher != null)
//                Claims.Add(new Claim("TeacherId", user.Teacher.Id.ToString()));
//            if (user.Parent != null)
//                Claims.Add(new Claim("ParentId", user.Parent.Id.ToString()));

//            foreach (var role in roles)
//            {
//                Claims.Add(new Claim(ClaimTypes.Role, role));
//            }

//            // 1 access token
//            SecurityToken token = new JwtSecurityToken(
//                issuer: configuration["JWT:ValidIssuer"],
//                audience: configuration["JWT:ValidAudience"],
//                claims: Claims,
//                expires: DateTime.UtcNow.AddMinutes(30),
//                signingCredentials: new SigningCredentials(
//                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
//                    SecurityAlgorithms.HmacSha256)
//            );
//            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

//            // 2 refresh token
//            //var refreshToken = Guid.NewGuid().ToString();
//            var refreshToken = "ggggg";

//            // 3. Store refresh token to Users table
//            user.RefreshToken = refreshToken;
//            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//            await userManager.UpdateAsync(user);

//            return new TokenDTO(accessToken, refreshToken, token.ValidTo, user.RefreshTokenExpiryTime);
//        }

//        public async Task<AuthResult> GenerateRefreshTokenAsync(TokenDTO request)
//        {
//            // 1. Validate the expired access token (even if expired)
//            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
//            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

//            // 2. Check if the refresh token exists & is valid
//            var user = await userManager.FindByIdAsync(userId);
//            if (user.RefreshToken == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
//                return new AuthResult
//                {
//                    Succeeded = false,
//                    Message = "Invalid token."
//                };

//            // 3. Generate new tokens
//            var newTokens = await GenerateTokenAsync(user);

//            // 4. Update the user's refresh token and expiry time
//            user.RefreshToken = newTokens.RefreshToken;
//            user.RefreshTokenExpiryTime = newTokens.RefreshTokenExpiration;
//            var updateResult = await userManager.UpdateAsync(user);

//            return new AuthResult
//            {
//                Succeeded = true,
//                AccessToken = newTokens.AccessToken,
//                AccessTokenExpiration = newTokens.AccessTokenExpiration,
//                RefreshToken = newTokens.RefreshToken,
//                RefreshTokenExpiration = newTokens.RefreshTokenExpiration
//                //Message = $"Access Token: [{newTokens.AccessToken}],"
//                //+ $"Expiry: [{newTokens.AccessTokenExpiration}], "
//                //+ $" Refresh Token: [{newTokens.RefreshToken}]"
//                //+ $"Expiry: [{newTokens.RefreshTokenExpiration}]"
//            };
//        }

//        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
//        {
//            var tokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
//                ValidIssuer = configuration["Jwt:Issuer"],
//                ValidAudience = configuration["Jwt:Audience"],
//                ValidateLifetime = false
//            };

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
//            return principal;
//        }






//    }
//}