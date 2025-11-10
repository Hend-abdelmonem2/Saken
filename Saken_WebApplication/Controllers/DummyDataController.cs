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
        
        private readonly IDummyUserService _dummyUserService;
        private readonly IDummyHouseService _dummyHouseService;
     


        public DummyDataController( IDummyUserService dummyUserService, IDummyHouseService dummyHouseService)
        {
           
            _dummyUserService = dummyUserService;
            _dummyHouseService = dummyHouseService;
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

        [HttpPost("insert-dummy-houses")]
        public async Task<IActionResult> InsertDummyHouses()
        {
            var result = await _dummyHouseService.InsertDummyHousesAsync();
            return Ok(result);
        }

        [HttpDelete("delete-dummy-houses")]
        public async Task<IActionResult> DeleteDummyHouses()
        {
            var result = await _dummyHouseService.DeleteDummyHousesAsync();
            return Ok(result);
        }

    }

}
