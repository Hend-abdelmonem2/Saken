using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Service.Services.Interfaces;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("settings")]
        public async Task<ActionResult<UpdateUserSettingsDto>> GetSettings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var settings = await _userService.GetSettingsAsync(userId);

            if (settings == null) return NotFound();

            return Ok(settings);
        }
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateUserSettingsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _userService.UpdateSettingsAsync(userId, dto);

            if (!success) return NotFound("User not found");

            return Ok("Settings updated successfully");
        }
    }
}
