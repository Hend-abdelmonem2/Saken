using MediatR;
using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Message.Query.Models
{
    public class GetChatQuery : IRequest<BaseResponse<GetMessageDto>>
    {
        public string LoginUserId { get; set; }
        public string OtherUserId { get; set; }

        public GetChatQuery(string loginUserId, string otherUserId)
        {
            LoginUserId = loginUserId;
            OtherUserId = otherUserId;
        }
    }
}
