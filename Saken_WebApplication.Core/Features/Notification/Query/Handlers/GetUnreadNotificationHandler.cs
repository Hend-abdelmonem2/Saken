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
    public class GetUnreadNotificationsHandler : IRequestHandler<GetUnreadNotificationsQuery, BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Notification>>>
    {
        private readonly INotificationService _service;
        public GetUnreadNotificationsHandler(INotificationService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Notification>>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
            => await _service.GetUnreadNotificationsAsync(request.UserId);
    }
}
