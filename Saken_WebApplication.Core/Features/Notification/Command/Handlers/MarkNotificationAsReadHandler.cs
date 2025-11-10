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
    public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsReadCommand, BaseResponse<bool>>
    {
        private readonly INotificationService _service;

        public MarkNotificationAsReadHandler(INotificationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<bool>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
            => await _service.MarkAsReadAsync(request.Id);
    }
}
