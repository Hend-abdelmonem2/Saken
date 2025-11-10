using MediatR;
using Saken_WebApplication.Core.Features.Notification.Query.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Notification.Query.Handlers
{
    public class GetNotificationCountHandler : IRequestHandler<GetNotificationCountQuery, BaseResponse<int>>
    {
        private readonly INotificationService _service;

        public GetNotificationCountHandler(INotificationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<int>> Handle(GetNotificationCountQuery request, CancellationToken cancellationToken)
            => await _service.GetAllNotificationsAsync();
    }
}
