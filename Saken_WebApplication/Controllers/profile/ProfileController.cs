using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Profile.Command.Models;
using Saken_WebApplication.Core.Features.Profile.Query.Models;
using Saken_WebApplication.Data.DTO;

namespace Saken_WebApplication.Controllers.profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("UserProfile")]
        [Authorize]

        public async Task<IActionResult> GetLandlord(string userId)
        {
            var result = await _mediator.Send(new GetUserProfileQuery(userId));
            if (!result.Success)
                return NotFound();
            return Ok(result);
        }


        [HttpGet("TenantProfile")]
        [Authorize]

        public async Task<IActionResult> GetTenant(string userId)
        {
            var result = await _mediator.Send(new GetTenantProfileQuery(userId));
            if (!result.Success)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> Update(string userId, UpdateUserDto updateUser)
        {
            var result = await _mediator.Send(new UpdateProfileCommand(userId, updateUser));

            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

    }

}
