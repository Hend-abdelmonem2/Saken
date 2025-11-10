using Saken_WebApplication.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Infrasturcture.Repositories.Interfaces.Notifications
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUnreadAsync(string userId);
        Task MarkAsReadAsync(int id);
        Task SaveChangesAsync();
        Task<int> GetNotificationCountAsync();
    }
}
