using MediatR;
using Saken_WebApplication.Data.Models.Guid;
using Saken_WebApplication.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Agent.Query.Models
{
    public record GetByHousingIdQuery(int HousingId)
    : IRequest<BaseResponse<CommissionTracking>>;
}
