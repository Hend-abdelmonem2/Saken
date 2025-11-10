using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Reservation.Command.Models;
using Saken_WebApplication.Core.Features.Reservation.Query.Models;
using Saken_WebApplication.Data.DTO.HousingDTO;
using System.Security.Claims;

namespace Saken_WebApplication.Controllers.Reservation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public ReservationController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("AddReservation")]
        [Authorize]
        public async Task<IActionResult> AddReservation([FromBody] ReservationDto dto)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new AddReservationCommand(dto, userId));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("confirmReservation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int reservationId)
        {
            var landlordId = GetUserId();
            if (landlordId == null)
                return Unauthorized("User ID not found in token.");

            var result=await _mediator.Send(new ConfirmReservationCommand(reservationId, landlordId));
            if (!result.Success)
                return NotFound(result);
            return Ok("✅ تم تأكيد الحجز بنجاح");
        }

        [HttpDelete("cancelReservation/{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;
            var result= await _mediator.Send(new CancelReservationCommand(id, userId));
            if (!result.Success)
                return NotFound(result);
                return Ok(result);
           
        }

        [HttpGet("GetReservationByID/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetReservationByIdQuery(id));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("ReservationFortenant")]
        [Authorize]
        public async Task<IActionResult> GetForTenant()
        {
            var tenantId = GetUserId();
            var result = await _mediator.Send(new GetReservationsForTenantQuery(tenantId));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("ReservationForlandlord")]
        [Authorize]
        public async Task<IActionResult> GetForLandlord()
        {
            var landlordId =GetUserId();
            var result = await _mediator.Send(new GetReservationsForLandlordQuery(landlordId));
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("Reservationcontract/{id}")]
        [Authorize]
        public async Task<IActionResult> GetContract(int id)
        {
            var contract = await _mediator.Send(new GetReservationContractQuery(id));
            if (!contract.Success)
                return NotFound(contract);
            return Ok(contract);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("AllReservation")]
        public async Task<IActionResult> GetAllReservation()

        {
            var result=await _mediator.Send(new GetAllReservationQuery());
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("GetHouseAvailableFrom")]
        [Authorize]
        public async Task<IActionResult> GetAvailableFrom(int id)

        {
            var result=await _mediator.Send(new GetAvailableFromQuery(id));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("ConfirmReservation")]
        [Authorize]
        public async Task<IActionResult> GetConfirmReservation(string id)
        {
            var result=await _mediator.Send(new GetConfirmedReservationsForTenantQuery(id));
            if(!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}
