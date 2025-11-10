using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Notification.Command.Models;
using Saken_WebApplication.Core.Features.Notification.Query.Models;

namespace Saken_WebApplication.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command)
        {
            var result = await _mediator.Send(command);
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{userId}/unreadNotification")]
        public async Task<IActionResult> GetUnread(string userId)
        {
            var result = await _mediator.Send(new GetUnreadNotificationsQuery(userId));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _mediator.Send(new MarkNotificationAsReadCommand(id));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var result = await _mediator.Send(new GetNotificationCountQuery());
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}
