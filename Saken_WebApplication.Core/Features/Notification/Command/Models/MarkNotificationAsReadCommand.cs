using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Notification.Command.Models
{
    public record MarkNotificationAsReadCommand(int Id) : IRequest<BaseResponse<bool>>;
}
