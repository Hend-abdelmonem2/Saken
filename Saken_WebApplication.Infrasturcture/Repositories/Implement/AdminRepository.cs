using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Implement
{
    public  class AdminRepository: IAdminRepository
    {
        private readonly UserManager<User> _userManager;
       

        public AdminRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            if (users == null || !users.Any())
            {
                throw new Exception("No Users Founded!");
            }
            var usersDto = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.UserName,
                Email = user.Email,
                profilePicture = user.profilePicture,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
            }).ToList();

            return usersDto;
        }
        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null!;
            var userDto = new UserDto
            {
                Id = user.Id,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                profilePicture = user.profilePicture

                // ضيفي أي خصائص إضافية موجودة في UserDto هنا
            };

            return userDto;
        }
        public async Task <List<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _userManager.Users
               .Where(u => u.Role == roleName)
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

        public async Task<(bool IsSuccess, string Message)> UpdateUserAsync(string userId, UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "المستخدم غير موجود");

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
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
        
        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return "User not found";

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return $"Failed to delete user: {errors}";
            }

            return "User deleted successfully";
        }
        public async Task<string> FreezeUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return "تم تجميد المستخدم";
        }

        public async Task<string> UnfreezeUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found";

            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            return "تم تفعيل المستخدم";
        }



    }
    }
