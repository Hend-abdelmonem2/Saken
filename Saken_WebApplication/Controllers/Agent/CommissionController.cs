using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Agent.Command.Models;
using Saken_WebApplication.Core.Features.Agent.Query.Models;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Controllers.Agent
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommissionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CommissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("agent/{agentId}")]
        public async Task<IActionResult> GetCommissionSteps(string agentId)
        {
            var result = await _mediator.Send(new GetCommissionStepsQuery(agentId));
            return Ok(result);
        }

        [HttpGet("housing/{housingId}")]
        public async Task<IActionResult> GetByHousingId(int housingId)
        {
            var result = await _mediator.Send(new GetByHousingIdQuery(housingId));
            return Ok(result);
        }

        [HttpPut("{commissionId}/status")]
        public async Task<IActionResult> UpdateStatus(int commissionId, [FromBody] CommissionStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateCommissionStatusCommand(commissionId, newStatus));
            return Ok(result);
        }
    }
}
