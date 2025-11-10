using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Data.Response;
using Saken_WebApplication.Service.Services.Interfaces.housing;
using Saken_WebApplication.Service.Services.Interfaces.recommend;
using Saken_WebApplication.Service.Services.Interfaces.Reservation;
using System.Security.Claims;
using static Saken_WebApplication.Data.Models.Enums;

namespace Saken_WebApplication.Controllers.Housing
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousingController : ControllerBase
    {

        private readonly IHousingService _housingService;
        private readonly IRecommendationService _recommendationService;
        private readonly IReservationService _reservationService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HousingController(IReservationService reservationService, IHousingService housingService, IRecommendationService recommendationService, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {

            _housingService = housingService;
            _recommendationService = recommendationService;
            _reservationService = reservationService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;


        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddHouse([FromForm] HousingDto dto)
        {
            var landlordId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(landlordId))
                return Unauthorized(new BaseResponse<string>(false, " User not authorized"));

            var response = await _mediator.Send(new AddHousingCommand(dto, landlordId));

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("updateHouse/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateHousingDto dto)
        {
            var userId = GetUserId();

            var result = await _mediator.Send(new UpdateHousingCommand(id, dto, userId));

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("DeleteHouse/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _mediator.Send(new DeleteHousingCommand(id, userId, isAdmin));

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("freeze/{id}")]
        [Authorize]
        public async Task<IActionResult> ToggleFreeze(int id)
        {
            var response = await _mediator.Send(new ToggleFreezeCommand(id));

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("AllHouses")]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetAllHousesQuery(userId));

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [Authorize]
        [HttpPost("request-inspection")]
        public async Task<IActionResult> RequestInspection([FromBody] InspectionRequestDto dto)
        {

            var response = await _mediator.Send(new SubmitInspectionRequestCommand(dto));
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("available-slots/{housingId}")]
        public async Task<IActionResult> GetAvailableSlots(int housingId)
        {
            var result = await _mediator.Send(new GetAvailableSlotsQuery(housingId));

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("landlord/grouped-housings/{landlordId}")]
        public async Task<IActionResult> GetGroupedHousingsForLandlord(string landlordId)
        {
            var response = await _mediator.Send(new GetGroupedHousingsForLandlordQuery(landlordId));

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("GetHouseById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetHousingById(int id)
        {
            var response = await _mediator.Send(new GetHousingByIdQuery(id));

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("highest-rating")]
        public async Task<IActionResult> GetHousingsByHighestRating()
        {
            var userId = GetUserId();
            var response = await _mediator.Send(new GetHousingsByHighestRatingQuery(userId));

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
        [HttpGet("lowest-price")]
        public async Task<IActionResult> LowestPriceAsync()
        {
            var userId = GetUserId();
            var response = await _mediator.Send(new GetHousingsByLowestPriceQuery(userId));
            if (!response.Success)
                return NotFound(response);
            return Ok(response);

        }

        [HttpGet("By-type/{type}")]
        [Authorize]
        public async Task<IActionResult> ByType(PropertyType type)
        {
            var userId = GetUserId();
            var response = await _mediator.Send(new GetHousingsByTypeQuery(userId, type));
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }

        [HttpGet("owner/inspection-requests")]
        [Authorize]
        public async Task<IActionResult> GetInspectionRequestsForOwner()
        {
            var ownerId = GetUserId();
            var response = await _mediator.Send(new GetInspectionRequestsForOwnerQuery(ownerId));

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("SaveHousing")]
        [Authorize]
        public async Task<IActionResult> SaveHousing(int houseId)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new SaveHousingCommand(userId, houseId));

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("savedHousing")]
        [Authorize]
        public async Task<IActionResult> Saved()
        {
            var userId = GetUserId();

            var result = await _mediator.Send(new GetSavedHousingsQuery(userId));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] string searchKey)
        {
            var result = await _mediator.Send(new SearchHousesQuery(searchKey));
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("search-by-address")]
        [Authorize]
        public async Task<IActionResult> SearchByAddress([FromQuery] string? address, [FromQuery] double? lat, [FromQuery] double? lng, [FromQuery] double radiusKm = 10)
        {
            var result = await _mediator.Send(new SearchByAddressQuery(address, lat, lng, radiusKm));
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("filter")]
        [Authorize]
        public async Task<IActionResult> Filter([FromQuery] string? address, [FromQuery] string? housingType, [FromQuery] string? furnishingStatus, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var result = await _mediator.Send(new GetFilteredHousesQuery(address, housingType, furnishingStatus, minPrice, maxPrice));
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }



        [HttpGet("HouseCost")]
        [Authorize]
        public async Task<IActionResult> GetCost(int id, int durationInMonth)
        {
            var result = await _mediator.Send(new GetHousingCostsQuery(id, durationInMonth));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }


    }
}
