using Saken_WebApplication.Data.DTO.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.message
{
   public  interface IMessageService
    {
        Task SendMessageAsync(MessageDto dto);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(string userId1, string userId2);
    }
}
