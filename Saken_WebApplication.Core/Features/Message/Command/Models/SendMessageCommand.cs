using MediatR;
using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Message.Command.Models
{
    public class SendMessageCommand : IRequest<BaseResponse>
    {
        public string UserId { get; set; }
        public MessageDto Message { get; set; }

        public SendMessageCommand(string userId, MessageDto message)
        {
            UserId = userId;
            Message = message;
        }
    }
}
