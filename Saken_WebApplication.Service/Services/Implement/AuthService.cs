using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Service.Services.Implement
{
    public class AuthService:IAuthService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
    
        public AuthService(
            IUserRepository userRepository,
            ICloudinaryService cloudinaryService,
            ITokenService tokenService,
            Microsoft.AspNetCore.Identity.UserManager<User> userManager,
           IEmailService emailService
            

         )
        {
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
          
        }
        public async Task<AuthModel> RegisterAsync(RegisterModelDTO model)
        {
            var existingUserWithRole = await _userRepository.FindByEmailAndRoleAsync(model.Email, model.Role);
            if (existingUserWithRole != null)
                return new AuthModel { Message = "Email is already registered with this role!" };

            var existingUser = await _userRepository.FindByUsernameAsync(model.FullName);
            if (existingUser != null)
            {
                string newUsername;
                do
                {
                    newUsername = $"{model.FullName}{new Random().Next(1000, 9999)}";
                    existingUser = await _userRepository.FindByUsernameAsync(newUsername);
                } while (existingUser != null);

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
                createdAt = DateTime.UtcNow,
                RefreshTokens = new List<RefreshToken>()
            };

          
            if (model.Photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(model.Photo);
                if (uploadResult.Error != null)
                    return new AuthModel { Message = uploadResult.Error.Message };

                user.profilePicture = uploadResult.SecureUrl.ToString();
            }
            else if (!string.IsNullOrEmpty(model.PhotoUrl))
            {
                user.profilePicture = model.PhotoUrl;
            }

            // ✅ إنشاء المستخدم في Identity
            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
                return new AuthModel { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };


            await _userRepository.AddToRoleAsync(user, model.Role.ToUpper());
            // ✅ إنشاء التوكن
            var token = await _tokenService.CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new AuthModel
            {
                Message = "User registered successfully!",
                Email = user.Email,
                //ExpiresOn = token.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { user.Role },
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.UserName,
                PhotoUrl = user.profilePicture
            };
        }
        public async Task<AuthModel> LoginAsync(RequestLoginDto request)
        {
            var authModel = new AuthModel();

            var user = await _userRepository.FindByEmailAndRoleAsync(request.Email, request.Role);
            if (user == null)
                return new AuthModel { Message = "Invalid email or role." };

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await _tokenService.CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

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

            return authModel;
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var authModel = new AuthResponseDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
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

            return authModel;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
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
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();
            if (users == null || !users.Any())
            {
                throw new Exception("No Users Founded!");
            }
            var usersDto = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName= user.UserName,
                Email = user.Email,
                profilePicture = user.profilePicture,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
            }).ToList();

            return usersDto;
        }
        public async Task<(bool IsSuccess, string Message)> UpdateProfileAsync(string userId, UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "المستخدم غير موجود");

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            if (model.photo != null)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(model.photo);

                if (uploadResult.Error != null)
                    return (false, uploadResult.Error.Message);

                user.profilePicture = uploadResult.SecureUrl.ToString();
            }

            // ✅ تعديل الباسورد (لو تم إرسال باسورد جديد)
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return (false, "فشل في إزالة كلمة المرور القديمة");

                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                    return (false, "كلمة المرور الجديدة غير صالحة");
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return (false, "فشل في تحديث البيانات");

            return (true, "تم تحديث الملف الشخصي بنجاح");
        
        }
        public async Task UpdateRoleAsync(UpdateRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId)
                ?? throw new Exception("User not found");
            var currentRoles = await _userManager.GetRolesAsync(user);
            // إزالة الأدوار الحالية
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to remove roles: {errors}");
                }
            }
            user.Role = model.NewRoleName;
            // إضافة الأدوار الجديدة
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add roles: {errors}");
            }
        }
        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.Users
                .Where(u => u.Role == role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.UserName,
                    Email = u.Email,
                    Role = u.Role,
                    PhoneNumber=u.PhoneNumber,
                    profilePicture=u.profilePicture
                })
                .ToListAsync();

            return users;
        }
        public async Task<UserDto> GetUserByIdAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id)
                 ?? throw new Exception("User not found");

            var userDto = new UserDto
            {
                Id = user.Id,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                profilePicture=user.profilePicture

                // ضيفي أي خصائص إضافية موجودة في UserDto هنا
            };

            return userDto;
        }


        private string GenerateCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            var resetCode = GenerateCode();

            user.ResetCode = resetCode;
            await _userManager.UpdateAsync(user);

            var subject = "Password Reset Code";
            var message = $"Your password reset code is: {resetCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return $"A password reset code has been sent to your email.";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email) ??
               throw new Exception("User not found");

            //  التحقق من الرمز
            if (user.ResetCode != model.ResetCode)
                throw new Exception("Invalid reset code");

            // التحقق مما إذا كانت كلمة المرور الجديدة هي نفس القديمة
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.NewPassword);
            if (passwordCheck)
                throw new Exception("New password cannot be the same as the current password.");

            //  إعادة تعيين كلمة المرور
            var resetPassword = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to reset password: {errors}");
            }

            //  إزالة الرمز بعد الاستخدام
            user.ResetCode = null;
            await _userManager.UpdateAsync(user);

            return "Password has been reset successfully!";
        }

        public async Task<string> Send2FACodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            // التحقق مما إذا كان هناك كود موجود ولكنه منتهي الصلاحية
            if (user.TwoFactorCodeExpiration != null && user.TwoFactorCodeExpiration > DateTime.Now)
                return "A valid 2FA code has already been sent. Please check your email.";

            var twoFactorCode = GenerateCode();

            user.TwoFactorCode = twoFactorCode;
            user.TwoFactorCodeExpiration = DateTime.Now.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            var subject = "Your 2FA Code";
            var message = $"Your Two-Factor Authentication code is : {twoFactorCode}";
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return "A 2FA code has been sent to your email.";
        }

        public async Task<string> Verify2FACodeAsync(Verify2FACodeDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email)
             ?? throw new Exception("User not found");

            // التحقق مما إذا كان الكود قد انتهت صلاحيته
            if (user.TwoFactorCode == null || user.TwoFactorCodeExpiration < DateTime.UtcNow)
                throw new Exception("The 2FA code has expired. Please request a new one.");

            // التحقق من صحة الكود
            if (user.TwoFactorCode != model.Code)
            {
                user.FailedTwoFactorAttempts++;

                // إذا تجاوز 5 محاولات خاطئة، يتم قفل الحساب
                if (user.FailedTwoFactorAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(10); //  قفل الحساب لمدة 10 دقيقة
                    await _userManager.UpdateAsync(user);
                    throw new Exception("Too many failed attempts. Your account is locked for 10 minutes.");
                }

                await _userManager.UpdateAsync(user);
                throw new Exception("Invalid 2FA code.");
            }

            // Reset the 2FA code after successful verification
            user.FailedTwoFactorAttempts = 0;
            user.TwoFactorAttempts = 0;
            user.LockoutEnd = null;
            user.TwoFactorCode = null;
            user.TwoFactorCodeExpiration = null;

            await _userManager.UpdateAsync(user);

            return "2FA verification successful";
        }

        public async Task<string> Resend2FACodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ??
                throw new Exception("User not found");

            if (user.TwoFactorSentAt != null && (DateTime.UtcNow - user.TwoFactorSentAt.Value).TotalSeconds < 60)
            {
                throw new Exception("Please wait at least 1 minute before requesting a new code.");
            }

            // التحقق مما إذا كان الحساب مقفلًا حاليًا
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                throw new Exception($"Your account is locked. Try again at {user.LockoutEnd.Value.ToLocalTime()}.");
            }

            // إعادة ضبط المحاولات إذا مرت ساعة
            if (user.LastTwoFactorAttempt != null && (DateTime.UtcNow - user.LastTwoFactorAttempt.Value).TotalHours >= 1)
            {
                user.TwoFactorAttempts = 0;
            }

            if (user.TwoFactorAttempts >= 5)
            {
                throw new Exception("You have exceeded the maximum number of attempts. Please try again later.");
            }

            var newCode = GenerateCode();
            user.TwoFactorCode = newCode;
            user.TwoFactorCodeExpiration = DateTime.UtcNow.AddMinutes(10);

            user.TwoFactorSentAt = DateTime.UtcNow;

            user.TwoFactorAttempts += 1;
            user.LastTwoFactorAttempt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            await _emailService.SendEmailAsync(user.Email, "Your 2FA Code", $"Your new 2FA code is: {newCode}");

            return "A new 2FA code has been sent to your email.";
        }




    }
}
