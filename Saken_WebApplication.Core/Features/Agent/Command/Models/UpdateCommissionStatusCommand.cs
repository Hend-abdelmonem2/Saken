using MediatR;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Core.Features.Agent.Command.Models
{
    public record UpdateCommissionStatusCommand(int CommissionId, CommissionStatus NewStatus)
    : IRequest<BaseResponse<bool>>;
}
