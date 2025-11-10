using Microsoft.AspNetCore.Identity;
using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Infrasturcture.Repositories.Implement.Notifications;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Notifications;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Implement.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<User> _userManager;
        public NotificationService(INotificationRepository notificationRepository, UserManager<User> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public async Task<BaseResponse<bool>> SendNotificationAsync(string userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return BaseResponse<bool>.SuccessResponse(true, "Notification sent successfully");
        }

        public async Task<BaseResponse<bool>> SendNotificationToAdminsAsync(string title, string message)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in admins)
            {
                await SendNotificationAsync(admin.Id, title, message);
            }
            return BaseResponse<bool>.SuccessResponse(true, "Notifications sent to all admins");
        }

        public async Task<BaseResponse<IEnumerable<Notification>>> GetUnreadNotificationsAsync(string userId)
        {
            var notifications = await _notificationRepository.GetUnreadAsync(userId);
            return BaseResponse<IEnumerable<Notification>>.SuccessResponse(notifications);
        }

        public async Task<BaseResponse<bool>> MarkAsReadAsync(int id)
        {
            await _notificationRepository.MarkAsReadAsync(id);
            await _notificationRepository.SaveChangesAsync();
            return BaseResponse<bool>.SuccessResponse(true, "Notification marked as read");
        }

        public async Task<BaseResponse<int>> GetAllNotificationsAsync()
        {
            var count = await _notificationRepository.GetNotificationCountAsync();
            return BaseResponse<int>.SuccessResponse(count);
        }
    }
    }


