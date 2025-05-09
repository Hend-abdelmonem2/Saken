using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO.message;
using Saken_WebApplication.Service.Services.Interfaces.message;

namespace Saken_WebApplication.Controllers.message
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _messageService.SendMessageAsync(dto);
            return Ok(new { message = "Message sent successfully" });
        }
        [HttpGet("getMessage/{userId1}/{userId2}")]
        public async Task<IActionResult> GetMessages(string userId1, string userId2)
        {
            var messages = await _messageService.GetMessagesAsync(userId1, userId2);
            return Ok(messages);
        }

    }
}
