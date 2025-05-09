using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO.UserPreferences;
using Saken_WebApplication.Service.Services.Interfaces.UserPreferences;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Preference
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IUserPreferencesService _service;

        public PreferenceController(IUserPreferencesService service)
        {
            _service = service;
        }
        [HttpPost("AddPreferences")]
        public async Task<IActionResult> Save([FromForm] UserPreferencesDto  model)
        {
            await _service.SavePreferencesAsync( model);
            return Ok(new { message = "Preferences saved successfully." });
        }
        [Authorize]
        [HttpGet("preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var prefs = await _service.GetPreferencesAsync(userId);

            if (prefs == null)
                return NotFound("Preferences not found.");

            return Ok(prefs);
        }
    }
}
