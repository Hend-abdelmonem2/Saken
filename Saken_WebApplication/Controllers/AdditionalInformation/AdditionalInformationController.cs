using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saken_WebApplication.Core.Features.AdditionalInformation.Command.Models;
using Saken_WebApplication.Core.Features.AdditionalInformation.Query.Models;
using Saken_WebApplication.Data.DTO.UserPreferences;

namespace Saken_WebApplication.Controllers.AdditionalInformation
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalInformationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdditionalInformationController(IMediator mediator)
        {
           _mediator = mediator;
        }

        [HttpPost("AddAditionalInformation")]
        [Authorize]
        public async Task<IActionResult> AddInformation([FromBody]SavePreferencesCommand command)
        {
            var result = await _mediator.Send(command); // ✅ هنا await مهم جدًا

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpGet("GetInformation")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetPreferencesQuery query)
        {
           
            var result = await _mediator.Send(query);

            if (result == null)
                return StatusCode(500, "حدث خطأ أثناء معالجة الطلب");

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
