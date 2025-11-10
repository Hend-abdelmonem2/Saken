using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Likes.Command.Models;
using Saken_WebApplication.Core.Features.Likes.Qyery.Models;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ToggleLike")]
        [Authorize]
        public async Task<IActionResult> ToggleLike([FromBody] ToggleLikeCommand command)
        {
            var result = await _mediator.Send(command);
            if(!result.Success) 
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("Likedhouses")]
        [Authorize]
        public async Task<IActionResult> GetLikedHouses()
        {
            var result = await _mediator.Send(new GetLikedHousesQuery());
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

      
        [HttpGet("Likedusers")]
        [Authorize]
        public async Task<IActionResult> GetLikedUsers()
        {
            var result = await _mediator.Send(new GetLikedUsersQuery());
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

    }
}
