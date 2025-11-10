using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Implement;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.Admin;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.Admin
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IHousingService _housingService;
        private readonly IReservationService _reservationService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        public AdminService(UserManager<User> userManager, IAdminRepository adminRepository,IHousingService housingService,IReservationService reservationService, INotificationService notificationService)
        {
            _adminRepository = adminRepository;
            _reservationService = reservationService;
            _notificationService = notificationService;
            _userManager = userManager;
            
        }


        public async Task<BaseResponse<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _adminRepository.GetAllUsersAsync();
            if (!users.Any())
                return BaseResponse<List<UserDto>>.Failure("No Users Found!");

            var dto = users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                profilePicture = user.profilePicture,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive=user.IsActive
               
            }).ToList();

            return BaseResponse<List<UserDto>>.SuccessResponse(dto, "Users retrieved successfully");
        }

        public async Task<BaseResponse<UserDto>> GetUserByIdAsync(string id)
        {
            var user = await _adminRepository.GetUserByIdAsync(id);
            if (user == null)
                return BaseResponse<UserDto>.Failure("User not found");

            var dto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                profilePicture = user.profilePicture,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive=user.IsActive
            };

            return BaseResponse<UserDto>.SuccessResponse(dto, "User retrieved successfully");
        }

        public async Task<BaseResponse<List<UserDto>>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _adminRepository.GetUsersByRoleAsync(roleName);

            var dto = users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                profilePicture = u.profilePicture,
                IsActive= u.IsActive,
            }).ToList();

            return BaseResponse<List<UserDto>>.SuccessResponse(dto, "Users retrieved successfully");
        }

        public async Task<BaseResponse<bool>> UpdateUserAsync(string userId, UpdateUserDto model)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return BaseResponse<bool>.Failure("User not found");

            
            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email != user.Email)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    return BaseResponse<bool>.Failure("Email is already in use by another user.");

                user.Email = model.Email;
               
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;

            
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return BaseResponse<bool>.Failure("Failed to remove old password");

                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                    return BaseResponse<bool>.Failure("Invalid new password");
            }

            var result = await _adminRepository.UpdateUserAsync(user);
            if (!result.Succeeded)
                return BaseResponse<bool>.Failure("Failed to update user");

            return BaseResponse<bool>.SuccessResponse(true, "User updated successfully");
        }


        public async Task<BaseResponse<bool>> DeleteUserAsync(string userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return BaseResponse<bool>.Failure("User not found");

            var result = await _adminRepository.DeleteUserAsync(user);
            if (!result.Succeeded)
                return BaseResponse<bool>.Failure("Failed to delete user");

            return BaseResponse<bool>.SuccessResponse(true, "User deleted successfully");
        }

        public async Task<BaseResponse<bool>> FreezeUserAsync(string userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return BaseResponse<bool>.Failure("User not found");

            user.IsActive = false;
            await _adminRepository.UpdateUserAsync(user);

            return BaseResponse<bool>.SuccessResponse(true, "User frozen successfully");
        }

        public async Task<BaseResponse<bool>> UnfreezeUserAsync(string userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return BaseResponse<bool>.Failure("User not found");

            user.IsActive = true;
            await _adminRepository.UpdateUserAsync(user);

            return BaseResponse<bool>.SuccessResponse(true, "User unfrozen successfully");
        }

    

        public async Task<BaseResponse<int>> GetUserCountAsync()
        {
            var users = await _adminRepository.GetCountUserAsync();
            if (users == null)
                return BaseResponse<int>.Failure("No User Founded");
            return BaseResponse<int>.SuccessResponse(users, "Retrive Users");
        }

        public async Task<BaseResponse<int>> GetReservationCountAsync()
        {
            var reservations = await _reservationService.GetAllReservationsCountAsync();
            if (reservations == null)
                return BaseResponse<int>.Failure("No Reservations Found");
            return BaseResponse<int>.SuccessResponse(reservations.Data, "Retrive Reservations");

        }

        public async Task<BaseResponse<int>> GetNotificationsCountAsync()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            if(notifications == null)
                return BaseResponse<int>.Failure("No Notifications Founded");
            return BaseResponse<int>.SuccessResponse(notifications.Data, "Retrive notifications");

        }

     

    }
}
