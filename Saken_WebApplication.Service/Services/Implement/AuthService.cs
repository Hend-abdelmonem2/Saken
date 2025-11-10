using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Data;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDBContext _dbContext;

        public AuthService(
            IUserRepository userRepository,
            ICloudinaryService cloudinaryService,
            ITokenService tokenService,
            Microsoft.AspNetCore.Identity.UserManager<User> userManager,
           IEmailService emailService,
            ApplicationDBContext dBContext

         )
        {
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
            _dbContext = dBContext;

        }
        public async Task<BaseResponse<AuthModel>> RegisterAsync(RegisterModelDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.PhoneNumber))
                return BaseResponse<AuthModel>.Failure("Email or Phone number is required!");

            if (string.IsNullOrEmpty(model.Role))
                return BaseResponse<AuthModel>.Failure("Role is required!");


            if (!string.IsNullOrEmpty(model.Email))
            {
                var existingUserWithRole = await _userRepository.FindByEmailAndRoleAsync(model.Email, model.Role);
                if (existingUserWithRole != null)
                    return BaseResponse<AuthModel>.Failure("Email is already registered with this role!");
            }


            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                var normalizedPhone = NormalizePhone(model.PhoneNumber);
                var existingUserWithPhone = await _userRepository.FindByPhoneAndRoleAsync(normalizedPhone, model.Role);
                if (existingUserWithPhone != null)
                    return BaseResponse<AuthModel>.Failure("Phone number is already registered with this role!");
            }
            if (string.IsNullOrEmpty(model.Email))
                model.Email = $"{Guid.NewGuid()}@placeholder.local";


            var existingUser = await _userRepository.FindByUsernameAsync(model.FullName);
            if (existingUser != null)
            {
                string newUsername;
                User checkUsername;

                do
                {
                    newUsername = $"{model.FullName}{new Random().Next(1000, 9999)}";
                    checkUsername = await _userRepository.FindByUsernameAsync(newUsername);
                } while (checkUsername != null);

                model.FullName = newUsername;
            }


            var user = new User
            {
                UserName = model.FullName,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                address = model.address,
                profilePicture = null,
                Role = model.Role,
                IsActive = false,
                createdAt = DateTime.UtcNow,
                RefreshTokens = new List<RefreshToken>()
            };


            if (model.Photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(model.Photo);
                if (uploadResult.Error != null)
                    return BaseResponse<AuthModel>.Failure(uploadResult.Error.Message);

                user.profilePicture = uploadResult.SecureUrl.ToString();
            }
            else if (!string.IsNullOrEmpty(model.PhotoUrl))
            {
                user.profilePicture = model.PhotoUrl;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _userRepository.CreateUserAsync(user, model.Password);
                if (!result.Succeeded)
                    return BaseResponse<AuthModel>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));


                await _userRepository.AddToRoleAsync(user, model.Role.ToUpper());


                var token = await _tokenService.CreateJwtToken(user);
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens?.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                await transaction.CommitAsync();

                var authModel = new AuthModel
                {
                    Message = "User registered successfully!",
                    Email = user.Email,
                    phone = user.PhoneNumber,
                    IsAuthenticated = true,
                    Roles = new List<string> { user.Role },
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Username = user.UserName,
                    PhotoUrl = user.profilePicture
                };

                return BaseResponse<AuthModel>.SuccessResponse(authModel, "User registered successfully!");
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                return BaseResponse<AuthModel>.Failure($"Registration failed: {ex.Message}");
            }
        }

        public async Task<BaseResponse<AuthModel>> LoginAsync(RequestLoginDto request)
        {
            if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.PhoneNumber))
                return BaseResponse<AuthModel>.Failure("Email or Phone number is required!");

            var authModel = new AuthModel();
            User user = null;


            if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userRepository.FindByEmailAndRoleAsync(request.Email, request.Role);
            }
            else if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var normalizedPhone = NormalizePhone(request.PhoneNumber);
                user = await _userRepository.FindByPhoneAndRoleAsync(request.PhoneNumber, request.Role);
            }

            if (user == null)
                return BaseResponse<AuthModel>.Failure("Invalid credentials or role.");


            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return BaseResponse<AuthModel>.Failure("Invalid credentials!");


            user.IsActive = true;
            await _userManager.UpdateAsync(user);


            var jwtSecurityToken = await _tokenService.CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.Message = "User Login Successfully";
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.phone = user.PhoneNumber;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.PhotoUrl = user.profilePicture;


            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return BaseResponse<AuthModel>.SuccessResponse(authModel, "Login Successfully");
        }
        private string NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.StartsWith("0"))
                digits = digits.Substring(1);

            return $"+20{digits}";
        }

        public async Task<BaseResponse> LogoutAsync(string? accessToken, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new BaseResponse(false, "User not found");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseResponse(false, "User not found");

            // 1️⃣ Revoke all refresh tokens
            if (user.RefreshTokens != null && user.RefreshTokens.Any(t => t.IsActive))
            {
                foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
                {
                    token.RevokedOn = DateTime.UtcNow;
                }
                user.IsActive = false;

                await _userManager.UpdateAsync(user);
            }



            return new BaseResponse(true, "Logout done");
        }


        public async Task<BaseResponse<AuthResponseDto>> RefreshTokenAsync(string token)
        {
            var authModel = new AuthResponseDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return BaseResponse<AuthResponseDto>.Failure("Invalid token");
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {

                return BaseResponse<AuthResponseDto>.Failure("Inactive token");
            }

            refreshToken.RevokedOn = DateTime.Now;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await _tokenService.CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return BaseResponse<AuthResponseDto>.SuccessResponse(authModel, "Refresh Token");
        }
        public async Task<BaseResponse<bool>> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return BaseResponse<bool>.Failure("User Not Found");

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return BaseResponse<bool>.Failure("Token Not Active");

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return BaseResponse<bool>.SuccessResponse(true, "Revoke Successfully");
        }
        public async Task<BaseResponse<IEnumerable<UserDto>>> GetUsersAsync(string userId)
        {
            var users = _userManager.Users.ToList();
            if (users == null || !users.Any())
            {
                return BaseResponse<IEnumerable<UserDto>>.Failure("No Users Founded!");
            }
            var likedIds = await _dbContext.Likes
              .Where(l => l.UserId == userId && l.EntityType == "User")
              .Select(l => l.EntityId)
              .ToListAsync();

            var usersDto = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.UserName,
                Email = user.Email,
                profilePicture = user.profilePicture,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                IsFavorite = likedIds.Contains(user.Id)

            }).ToList();

            return BaseResponse<IEnumerable<UserDto>>.SuccessResponse(usersDto, "Retrive Users Successfully");
        }
        public async Task<BaseResponse<string>> UpdateProfileAsync(string userId, UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BaseResponse<string>.Failure("User Not Found");


            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;


            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    return BaseResponse<string>.Failure("Email is already in use by another user.");

                user.Email = model.Email;

            }


            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber != user.PhoneNumber)
            {
                var existingPhoneUser = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                if (existingPhoneUser != null)
                    return BaseResponse<string>.Failure("Phone number is already in use by another user.");

                user.PhoneNumber = model.PhoneNumber;
            }


            if (model.photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(model.photo);
                if (uploadResult.Error != null)
                    return BaseResponse<string>.Failure(uploadResult.Error.Message);

                user.profilePicture = uploadResult.SecureUrl.ToString();
            }


            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return BaseResponse<string>.Failure("فشل في إزالة كلمة المرور القديمة");

                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                    return BaseResponse<string>.Failure("كلمة المرور الجديدة غير صالحة");
            }


            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BaseResponse<string>.Failure("فشل في تحديث البيانات");

            return BaseResponse<string>.SuccessResponse("تم تحديث الملف الشخصي بنجاح");
        }

        public async Task<BaseResponse<string>> UpdateRoleAsync(UpdateRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return BaseResponse<string>.Failure("User Not Found");
            var currentRoles = await _userManager.GetRolesAsync(user);
            // إزالة الأدوار الحالية
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return BaseResponse<string>.Failure($"Failed to remove roles: {errors}");
                }
            }
            user.Role = model.NewRoleName;
            // إضافة الأدوار الجديدة
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BaseResponse<string>.Failure($"Failed to add roles: {errors}");
            }
            return BaseResponse<string>.SuccessResponse("Update Role Successfully");
        }
        public async Task<BaseResponse<IEnumerable<UserDto>>> GetUsersByRoleAsync(string userId, string role)
        {


            var likedIds = await _dbContext.Likes
                .Where(l => l.UserId == userId && l.EntityType == "User")
                .Select(l => l.EntityId)
                .ToListAsync();

            var users = await _userManager.Users
                .Where(u => u.Role == role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.UserName,
                    Email = u.Email,
                    Role = u.Role,
                    PhoneNumber = u.PhoneNumber,
                    profilePicture = u.profilePicture,
                    IsActive = u.IsActive,
                    IsFavorite = likedIds.Contains(u.Id)
                })
                .ToListAsync();

            return BaseResponse<IEnumerable<UserDto>>.SuccessResponse(users, "Retrive Users By Role");
        }
        public async Task<BaseResponse<UserDto>> GetUserByIdAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return BaseResponse<UserDto>.Failure("User not found");

            var userDto = new UserDto
            {
                Id = user.Id,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                profilePicture = user.profilePicture,
                IsActive = user.IsActive

            };

            return BaseResponse<UserDto>.SuccessResponse(userDto, "User Retrive Successfully");
        }
        public async Task<BaseResponse<string>> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BaseResponse<string>.Failure("User not found");

            var resetCode = GenerateCode();

            user.ResetCode = resetCode;
            await _userManager.UpdateAsync(user);

            var subject = "Password Reset Code";
            var message = $"Your password reset code is: {resetCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return BaseResponse<string>.SuccessResponse($"A password reset code has been sent to your email.");
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BaseResponse<string>.Failure("User not found");

            //  التحقق من الرمز
            if (user.ResetCode != model.ResetCode)
                return BaseResponse<string>.Failure("Invalid reset code");

            // التحقق مما إذا كانت كلمة المرور الجديدة هي نفس القديمة
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.NewPassword);
            if (passwordCheck)
                return BaseResponse<string>.Failure("New password cannot be the same as the current password.");

            //  إعادة تعيين كلمة المرور
            var resetPassword = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BaseResponse<string>.Failure($"Failed to reset password: {errors}");
            }

            //  إزالة الرمز بعد الاستخدام
            user.ResetCode = null;
            await _userManager.UpdateAsync(user);

            return BaseResponse<string>.SuccessResponse("Password has been reset successfully!");
        }

        public async Task<BaseResponse<string>> Send2FACodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BaseResponse<string>.Failure("User not found");

            // التحقق مما إذا كان هناك كود موجود ولكنه منتهي الصلاحية
            if (user.TwoFactorCodeExpiration != null && user.TwoFactorCodeExpiration > DateTime.Now)
                return BaseResponse<string>.Failure("A valid 2FA code has already been sent. Please check your email.");

            var twoFactorCode = GenerateCode();

            user.TwoFactorCode = twoFactorCode;
            user.TwoFactorCodeExpiration = DateTime.Now.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            var subject = "Your 2FA Code";
            var message = $"Your Two-Factor Authentication code is : {twoFactorCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return BaseResponse<string>.SuccessResponse("A 2FA code has been sent to your email.");
        }

        public async Task<BaseResponse<string>> Verify2FACodeAsync(Verify2FACodeDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BaseResponse<string>.Failure("User not found");

            // التحقق مما إذا كان الكود قد انتهت صلاحيته
            if (user.TwoFactorCode == null || user.TwoFactorCodeExpiration < DateTime.UtcNow)
                return BaseResponse<string>.Failure("The 2FA code has expired. Please request a new one.");

            // التحقق من صحة الكود
            if (user.TwoFactorCode != model.Code)
            {
                user.FailedTwoFactorAttempts++;

                // إذا تجاوز 5 محاولات خاطئة، يتم قفل الحساب
                if (user.FailedTwoFactorAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(10); //  قفل الحساب لمدة 10 دقيقة
                    await _userManager.UpdateAsync(user);
                    return BaseResponse<string>.Failure("Too many failed attempts. Your account is locked for 10 minutes.");
                }

                await _userManager.UpdateAsync(user);
                return BaseResponse<string>.Failure("Invalid 2FA code.");
            }

            // Reset the 2FA code after successful verification
            user.FailedTwoFactorAttempts = 0;
            user.TwoFactorAttempts = 0;
            user.LockoutEnd = null;
            user.TwoFactorCode = null;
            user.TwoFactorCodeExpiration = null;

            await _userManager.UpdateAsync(user);

            return BaseResponse<string>.SuccessResponse("2FA verification successful");
        }

        public async Task<BaseResponse<string>> Resend2FACodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BaseResponse<string>.Failure("User not found");

            if (user.TwoFactorSentAt != null && (DateTime.UtcNow - user.TwoFactorSentAt.Value).TotalSeconds < 60)
            {
                return BaseResponse<string>.Failure("Please wait at least 1 minute before requesting a new code.");
            }

            // التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                return BaseResponse<string>.Failure($"Your account is locked. Try again at {user.LockoutEnd.Value.ToLocalTime()}.");
            }

            // إعادة ضبط المحاولات إذا مرت ساعة
            if (user.LastTwoFactorAttempt != null && (DateTime.UtcNow - user.LastTwoFactorAttempt.Value).TotalHours >= 1)
            {
                user.TwoFactorAttempts = 0;
            }

            if (user.TwoFactorAttempts >= 5)
            {
                return BaseResponse<string>.Failure("You have exceeded the maximum number of attempts. Please try again later.");
            }

            var newCode = GenerateCode();
            user.TwoFactorCode = newCode;
            user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10);

            user.TwoFactorSentAt = DateTime.UtcNow;

            user.TwoFactorAttempts += 1;
            user.LastTwoFactorAttempt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your new 2FA code is: {newCode}");

            return BaseResponse<string>.SuccessResponse("A new 2FA code has been sent to your email.");
        }

        private string GenerateCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }



    }
}
