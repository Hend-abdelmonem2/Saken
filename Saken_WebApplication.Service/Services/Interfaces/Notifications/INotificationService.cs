using Saken_WebApplication.Data.Models;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task<BaseResponse<bool>> SendNotificationAsync(string userId, string title, string message);
        Task<BaseResponse<bool>> SendNotificationToAdminsAsync(string title, string message);
        Task<BaseResponse<IEnumerable<Notification>>> GetUnreadNotificationsAsync(string userId);
        Task<BaseResponse<bool>> MarkAsReadAsync(int id);
        Task<BaseResponse<int>> GetAllNotificationsAsync();
    }
}
