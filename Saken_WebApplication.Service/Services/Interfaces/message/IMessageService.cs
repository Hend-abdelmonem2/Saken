using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Service.Services.Interfaces.message
{
   public  interface IMessageService
    {
        Task<BaseResponse> SendMessageAsync(string senderId, MessageDto dto);
        Task<BaseResponse<GetMessageDto>> GetChatAsync(string loginUserId, string otherUserId);
        Task<BaseResponse<IEnumerable<UserConversationDto>>> GetUserConversationsAsync(string userId);
    }
}
