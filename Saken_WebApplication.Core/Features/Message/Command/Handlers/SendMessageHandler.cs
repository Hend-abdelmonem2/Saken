using MediatR;
using Microsoft.AspNetCore.Http;
using Saken_WebApplication.Core.Features.Message.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Message.Command.Handlers
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, BaseResponse>
    {
        private readonly IMessageService _messageService;

        public SendMessageCommandHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<BaseResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            return await _messageService.SendMessageAsync(request.UserId, request.Message);
        }
    }
}
