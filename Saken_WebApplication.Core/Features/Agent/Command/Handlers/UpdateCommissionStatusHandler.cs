using MediatR;
using Saken_WebApplication.Core.Features.Agent.Command.Models;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saken_WebApplication.Core.Features.Agent.Command.Handlers
{
    public class UpdateCommissionStatusHandler
    : IRequestHandler<UpdateCommissionStatusCommand, BaseResponse<bool>>
    {
        private readonly IAgentCommissionService _service;

        public UpdateCommissionStatusHandler(IAgentCommissionService service)
        {
            _service = service;
        }

        public async Task<BaseResponse<bool>> Handle(UpdateCommissionStatusCommand request, CancellationToken cancellationToken)
        {
            return await _service.UpdateCommissionStatusAsync(request.CommissionId, request.NewStatus);
        }
    }
}
