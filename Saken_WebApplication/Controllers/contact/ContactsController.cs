using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Contacts.Command.Models;
using Saken_WebApplication.Core.Features.Contacts.Query.Models;

namespace Saken_WebApplication.Controllers.contact
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("SaveContact")]
        [Authorize]
        public async Task<IActionResult> AddContact([FromBody] AddContactCommand command)
        {
            var result = await _mediator.Send(command);
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

      
        [HttpGet("AllowedContact/{ownerUserId}")]
        [Authorize]
        public async Task<IActionResult> GetAllowedContacts(string ownerUserId)
        {
            var result = await _mediator.Send(new GetAllowedContactsQuery(ownerUserId));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

    
        [HttpGet("BlockedContact/{ownerUserId}")]
        [Authorize]
        public async Task<IActionResult> GetBlockedContacts(string ownerUserId)
        {
            var result = await _mediator.Send(new GetBlockedContactsQuery(ownerUserId));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

      
        [HttpPut("AddToblock")]
        [Authorize] 
        public async Task<IActionResult> BlockContact([FromBody] BlockContactCommand command)
        {
            var result = await _mediator.Send(command);
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}

