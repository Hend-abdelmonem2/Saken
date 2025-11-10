using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Message.Command.Models;
using Saken_WebApplication.Core.Features.Message.Query.Models;
using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Service.Services.Interfaces.message;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.message
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mediator.Send(new SendMessageCommand(userId, dto));
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("chat/{otherUserId}")]
        public async Task<IActionResult> GetChat(string otherUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mediator.Send(new GetChatQuery(userId, otherUserId));
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetUserConversations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mediator.Send(new GetUserConversationsQuery(userId));
            return response.Success ? Ok(response) : NotFound(response);
        }
    
}
    }
