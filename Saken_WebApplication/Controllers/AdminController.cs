using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.Admin.Command.Models;
using Saken_WebApplication.Core.Features.Admin.Query.Models;
using Saken_WebApplication.Core.Features.Houses.Command.Models;
using Saken_WebApplication.Core.Features.Houses.Query.Models;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Data.DTO.HousingDTO;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMediator _mediator;

        public AdminController(IAdminRepository adminRepository, IMediator mediator)
        {
            _adminRepository = adminRepository;
            _mediator = mediator;
        }

       
        [HttpGet("AllUsers")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            if (users==null)
                return NotFound();
            return Ok(users);
        }

        [HttpGet("User/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _mediator.Send(new  GetUserByIdQuery(id));
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

       
        [HttpGet("UsersByRole/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _mediator.Send(new GetUsersByRoleQuery(roleName));
            if (users==null)
                return NotFound();
            return Ok(users);
        }

        [HttpGet("GetUserCount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _mediator.Send(new GetUserCountQuery());
            if (count.Data==0)
                return NotFound();
            return Ok(count);
        }
        [HttpGet("GetNotificationCount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNotification()
        {
            var count = await _mediator.Send(new GetNotificationsCountQuery());
            if (count.Data==0)
                return NotFound();
            return Ok(count);
        }

        [HttpGet("GetReservationCount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReservation()
        {
            var count=await _mediator.Send(new GetReservationCountQuery());
            if(count.Data==0)
                return NotFound();
            return Ok(count);
        }

        [HttpPut("UpdateUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateUserCommand(userId, model));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

       
        [HttpDelete("DeleteUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand(userId));
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }


        [HttpPost("freeze/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FreezeUser(string userId)
        {
            var result = await _mediator.Send(new FreezeUserCommand(userId));
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("unfreeze/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnfreezeUser(string userId)
        {
            var result = await _mediator.Send(new UnfreezeUserCommand(userId));
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        /// <summary>
        /// موافقة على وحدة سكنية
        /// </summary>
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveHousing(int id)
        {
            var result = await _mediator.Send(new ApproveHousingCommand(id));
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// رفض وحدة سكنية مع ذكر السبب
        /// </summary>
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectHousing(int id, [FromBody] RejectRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                return BadRequest("سبب الرفض مطلوب");

            var result = await _mediator.Send(new RejectHousingCommand(id, dto.Reason));
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("countHouse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCountHouse()
        {
            var result = await _mediator.Send(new GetAllHousesCountQuery());
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("GetPendingHouse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>PendingHouse()
        {
            var pending= await _mediator.Send(new GetPendingHousesQuery());
            if(!pending.Success)
                return NotFound(pending);
            return Ok(pending);

        }

    }
}
