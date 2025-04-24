using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Housing
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousingController : ControllerBase
    {
        private readonly IHousingFilterService _housingFilterService;
        private readonly IHousingService _housingService;

        public HousingController(IHousingFilterService housingFilterService , IHousingService housingService)
        {
            _housingFilterService = housingFilterService;
            _housingService = housingService;
        }

   

        [HttpPost("add")]
        public async Task<IActionResult> AddHousing([FromForm] HousingDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _housingService.AddHousingAsync(dto, userId);
            return Ok(new { message = "Housing added successfully" });
        }
        [Authorize]
        [HttpPost("Reservation")]
        public async Task<IActionResult> AddReservation([FromBody] ReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // استخراج الـ UserId من التوكن
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User ID not found in token.");

            await _housingService.AddReservationAsync(dto, userId);
            return Ok(new { message = "Reservation created successfully" });
        }
        [HttpGet("AllHouses")]
        public async Task<IActionResult> GetAllHouses()
        {
            var result = await _housingService.GetAllHousesAsync();
            return Ok(result);
        }

        [HttpGet("recommendations/{userId}")]
        public async Task<ActionResult<List<HousingDto>>> GetRecommendations(string userId, [FromQuery] string? location)
        {
            var recommendations = await _housingFilterService.GetRecommendedHousingsAsync(userId, location);

            if (recommendations == null || !recommendations.Any())
                return NotFound("لا يوجد عقارات مناسبة لهذا المستخدم.");

            return Ok(recommendations);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHousing(int id, [FromForm] HousingDto dto)
        {
            await _housingService.UpdateHousingAsync(id, dto);
            return Ok("Housing updated successfully");
        }
    }
}
