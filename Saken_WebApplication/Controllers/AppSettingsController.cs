using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Service.Services.Interfaces;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingsService _settingsService;

        public AppSettingsController(IAppSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _settingsService.GetSettingsAsync();
            return Ok(settings);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] AppSettingsDto dto)
        {
            await _settingsService.UpdateSettingsAsync(dto);
            return NoContent();
        }
    }
}
