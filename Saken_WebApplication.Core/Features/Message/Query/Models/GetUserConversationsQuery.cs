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
    public class GetUserConversationsQuery : IRequest<BaseResponse<IEnumerable<UserConversationDto>>>
    {
        public string UserId { get; set; }

        public GetUserConversationsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
