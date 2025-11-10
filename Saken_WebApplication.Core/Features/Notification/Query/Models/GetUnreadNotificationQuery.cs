using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Notification.Query.Models
{
    public record GetUnreadNotificationsQuery(string UserId) : IRequest<BaseResponse<IEnumerable<Saken_WebApplication.Data.Models.Notification>>>;
}
