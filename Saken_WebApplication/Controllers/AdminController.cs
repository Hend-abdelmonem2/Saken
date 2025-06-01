using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Data.DTO;
using Saken_WebApplication.Infrasturcture.Repositories.Interfaces;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

       
        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _adminRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

       
        [HttpGet("UsersByRole/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _adminRepository.GetUsersByRoleAsync(roleName);
            return Ok(users);
        }

        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminRepository.UpdateUserAsync(userId, model);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

       
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminRepository.DeleteUserAsync(userId);
            if (result == "User not found")
                return NotFound(result);

         return Ok(result);
        }
        [HttpPost("freeze/{userId}")]
        public async Task<IActionResult> FreezeUser(string userId)
        {
            var result = await _adminRepository.FreezeUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("unfreeze/{userId}")]
        public async Task<IActionResult> UnfreezeUser(string userId)
        {
            var result = await _adminRepository.UnfreezeUserAsync(userId);
            return Ok(result);
        }

    }
}
