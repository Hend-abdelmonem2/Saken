using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Service.Services.Interfaces;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Housing
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousingController : ControllerBase
    {
        private readonly IHousingFilterService _housingFilterService;
        private readonly IHousingService _housingService;
        private readonly IRecommendationService _recommendationService;
        private readonly IReservationService _reservationService;

        public HousingController(IHousingFilterService housingFilterService, IReservationService reservationService, IHousingService housingService, IRecommendationService recommendationService)
        {
            _housingFilterService = housingFilterService;
            _housingService = housingService;
            _recommendationService = recommendationService;
            _reservationService = reservationService;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddHousing([FromForm] HousingDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();


                return BadRequest(new { message = "Model validation failed", errors });
            }

            await _housingService.AddHousingAsync(dto, userId);
            return Ok(new { message = "Housing added successfully" });
        }
        [Authorize]
        [HttpPost("Reservation")]
        public async Task<IActionResult> AddReservation([FromForm] ReservationDto dto)
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
        [HttpGet("landlordResrvation")]
        [Authorize]
        public async Task<IActionResult> GetLandlordReservations()
        {
            var landlordId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (landlordId == null)
                return Unauthorized();

            var reservations = await _reservationService.GetReservationsForLandlordAsync(landlordId);
            return Ok(reservations);
        }
        [HttpGet("reservation/{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationById(id);

            if (reservation == null)
                return NotFound("Reservation not found.");

            return Ok(reservation);
        }




        [HttpGet("AllHouses")]
        public async Task<IActionResult> GetAllHouses()
        {
            var result = await _housingService.GetAllHousesAsync();
            return Ok(result);
        }
        [HttpPut("updateHouse/{id}")]
        public async Task<IActionResult> UpdateHouse(int id, [FromForm] HousingDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User ID not found in token.");
            await _housingService.UpdateHousingAsync(id, dto);
            return Ok(new { message = "Housing updated successfully" });

        }

        [HttpGet("searchHouse")]
        public async Task<IActionResult> SearchHouses(string key)
        {
            var results = await _housingService.SearchHousesAsync(key);
            return Ok(results);
        }

        [HttpDelete("DeleteHouse/{id}")]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            var result = await _housingService.DeleteHousingAsync(id);
            if (!result)
                return NotFound("House not found");

            return Ok("House deleted successfully");
        }


        [HttpGet("recommendations/{userId}")]
        public async Task<ActionResult<List<HousingDto>>> GetRecommendations(string userId, [FromQuery] string? location)
        {
            var recommendations = await _housingFilterService.GetRecommendedHousingsAsync(userId, location);

            if (recommendations == null || !recommendations.Any())
                return NotFound("لا يوجد عقارات مناسبة لهذا المستخدم.");

            return Ok(recommendations);
        }
        [Authorize]
        [HttpGet("recommend")]
        public async Task<IActionResult> Recommend()
        {
            var result = await _recommendationService.GetRecommendedHousesAsync();
            return Ok(result);
        }
        [HttpGet("my-houses")]
        [Authorize]
        public async Task<IActionResult> GetMyHousings()
        {
            var landlordId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (landlordId == null)
                return Unauthorized();

            var housings = await _housingService.GetHousingsForLandlordIdAsync(landlordId);
            return Ok(housings);
        }

        [HttpGet("contract/{id}")]
        public async Task<IActionResult> GetReservationContract(int id)
        {
            var contract = await _reservationService.GetReservationContractAsync(id);
            if (contract == null)
                return NotFound("Reservation not found.");

            return Ok(contract);
        }
        [HttpPost("ToggleFreeze/{id}")]
        public async Task<IActionResult> ToggleFreeze(int id)
        {
            var result = await _housingService.ToggleFreezeAsync(id);

            if (!result.success)
                return NotFound(new { message = result.message });

            return Ok(new
            {
                message = result.message,
                isFrozen = result.isFrozen
            });
        }
        

    [HttpGet("GetHouseById/{id}")]
        public async Task<IActionResult> GetHouseById(int id)
        {
            var house = await _housingService.GetHousingByIdAsync(id);
            if (house == null)
                return NotFound("السكن غير موجود");

            return Ok(house);
        }
    }
}
