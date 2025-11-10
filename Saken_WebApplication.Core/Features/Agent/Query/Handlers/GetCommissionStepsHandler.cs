using MediatR;
using Saken_WebApplication.Core.Features.Agent.Query.Models;
using Saken_WebApplication.Data.Models.Guid;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Agent.Query.Handlers
{
    public class GetCommissionStepsHandler
      : IRequestHandler<GetCommissionStepsQuery, BaseResponse<List<CommissionTracking>>>
    {
        private readonly IAgentCommissionService _service;

        public GetCommissionStepsHandler(IAgentCommissionService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<List<CommissionTracking>>> Handle(GetCommissionStepsQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetCommissionStepsAsync(request.AgentId);
        }
    }
}
