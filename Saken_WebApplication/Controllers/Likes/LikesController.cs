using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO.Favorite;
using Saken_WebApplication.Service.Services.Interfaces.Like;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("ToggleLike")]
        public async Task<IActionResult> ToggleLike([FromBody] AddLikeDTO likeDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            var result = await _likeService.ToggleLikeAsync(userId, likeDto.EntityId, likeDto.EntityType);

            if (result == "Invalid entity type")
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });

        }
    }
}
