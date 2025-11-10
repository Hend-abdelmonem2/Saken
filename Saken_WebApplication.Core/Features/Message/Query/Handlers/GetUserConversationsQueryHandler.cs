using MediatR;
using Saken_WebApplication.Core.Features.Message.Query.Models;
using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Message.Query.Handlers
{
    public class GetUserConversationsQueryHandler : IRequestHandler<GetUserConversationsQuery, BaseResponse<IEnumerable<UserConversationDto>>>
    {
        private readonly IMessageService _messageService;

        public GetUserConversationsQueryHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<BaseResponse<IEnumerable<UserConversationDto>>> Handle(GetUserConversationsQuery request, CancellationToken cancellationToken)
        {
            return await _messageService.GetUserConversationsAsync(request.UserId);
        }
    }
}
