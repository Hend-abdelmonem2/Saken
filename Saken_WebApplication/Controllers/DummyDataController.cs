using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Service.Services.Implement;
using Saken_WebApplication.Service.Services.Interfaces;

namespace Saken_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyDataController : ControllerBase
    {
        private readonly IDummyDataService _service;
        private readonly IDummyUserService _dummyUserService;
     


        public DummyDataController(IDummyDataService service , IDummyUserService dummyUserService)
        {
            _service = service;
            _dummyUserService = dummyUserService;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetDummyData()
        {
            try
            {
                await _service.RunSqlScriptAsync("dummy_data.sql");
                return Ok("Dummy data set successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("reset")]
        public async Task<IActionResult> ResetDummyData()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "dummy_data.sql");
            await _service.RunSqlScriptAsync(path);
           
            return Ok("Dummy data reset successfully.");
        }
       

       

        [HttpPost("insert-users")]
        public async Task<IActionResult> InsertUsers()
        {
            var result = await _dummyUserService.InsertDummyUsersAsync();
            return Ok(result);
        }
        [HttpPost("reset-users")]
        public async Task<IActionResult> ResetUsers()
        {
            var result = await _dummyUserService.DeleteDummyUsersAsync();
            return Ok(result);
        }

    }

}
