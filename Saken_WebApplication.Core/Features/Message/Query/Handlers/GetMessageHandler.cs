using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class GetChatQueryHandler : IRequestHandler<GetChatQuery, BaseResponse<GetMessageDto>>
    {
        private readonly IMessageService _messageService;

        public GetChatQueryHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<BaseResponse<GetMessageDto>> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            return await _messageService.GetChatAsync(request.LoginUserId, request.OtherUserId);
        }
    }
}
