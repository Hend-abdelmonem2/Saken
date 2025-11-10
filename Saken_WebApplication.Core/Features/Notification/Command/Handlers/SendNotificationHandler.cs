using MediatR;
using Saken_WebApplication.Core.Features.Notification.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Notification.Command.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendNotificationCommand, BaseResponse<bool>>
    {
        private readonly INotificationService _service;
        public SendNotificationHandler(INotificationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<bool>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            => await _service.SendNotificationAsync(request.UserId, request.Title, request.Message);
    }
}
